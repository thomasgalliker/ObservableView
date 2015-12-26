using ObservableView.Extensions;
using ObservableView.Filtering;
using ObservableView.Grouping;
using ObservableView.Searching;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

using ObservableView.Sorting;

namespace ObservableView
{
    /// <summary>
    ///     ObservableView is a class which adds sorting, filtering, searching and grouping
    ///     on top of collections.
    /// </summary>
    public class ObservableView<T> : INotifyPropertyChanged, IObservableView
    {
        private static readonly object FilterHandlerEventLock = new object();

        private readonly List<OrderSpecification<T>> orderSpecifications;
        private readonly HashSet<PropertyInfo> searchSpecifications;
        private ObservableCollection<T> sourceCollection;

        private string searchText = string.Empty;
        private FilterEventHandler<T> filterHandler;
        private Func<T, string> groupKey;
        private IGroupKeyAlgorithm groupKeyAlogrithm;

        public ObservableView(ObservableCollection<T> collection)
        {
            this.Source = collection;
            this.orderSpecifications = new List<OrderSpecification<T>>();
            this.searchSpecifications = new HashSet<PropertyInfo>(this.GetSearchableAttributes());
            this.GroupKeyAlogrithm = new AlphaGroupKeyAlgorithm();
        }

        public ObservableView()
            : this(new ObservableCollection<T>())
        {
        }

        public ObservableView(IEnumerable<T> list)
            : this(list.ToObservableCollection())
        {
        }

        #region Public Events

        /// <summary>
        ///     The filter handler.
        /// </summary>
        public event FilterEventHandler<T> FilterHandler
        {
            add
            {
                lock (FilterHandlerEventLock)
                {
                    this.filterHandler += value;
                }
                this.Refresh();
            }
            remove
            {
                lock (FilterHandlerEventLock)
                {
                    this.filterHandler -= value;
                }
                this.Refresh();
            }
        }

        /// <summary>
        ///     The property changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Public Properties
        public Func<T, string> GroupKey
        {
            get
            {
                return this.groupKey;
            }
            set
            {
                this.groupKey = value;
                this.OnPropertyChanged(() => this.Groups);
            }
        }

        public IGroupKeyAlgorithm GroupKeyAlogrithm
        {
            get
            {
                return this.groupKeyAlogrithm;
            }
            set
            {
                this.groupKeyAlogrithm = value;
                this.OnPropertyChanged(() => this.Groups);
            }
        }

        public IEnumerable<Grouping<T>> Groups
        {
            get
            {
                if (this.GroupKey == null || this.GroupKeyAlogrithm == null)
                {
                    return Enumerable.Empty<Grouping<T>>();
                }

                var groupedList = this.View
                    .GroupBy(item => this.GroupKeyAlogrithm.GetGroupKey(this.GroupKey.Invoke(item)))
                    .Select(itemGroup => new Grouping<T>(itemGroup.Key, itemGroup))
                    .OrderBy(itemGroup => itemGroup.Key)
                    .ToList();

                return groupedList;
            }
        }

        /// <summary>
        ///     Gets or sets the search text.
        ///     This property can be used for data binding and has the same effect
        ///     as using the <code>Search("searchtext")</code> method to perform a search operation.
        /// </summary>
        public string SearchText
        {
            get
            {
                return this.searchText;
            }
            set
            {
                if (this.searchText != value)
                {
                    this.searchText = value;
                    this.OnPropertyChanged(() => this.SearchText);

                    // Update properties to reflect the search result
                    this.OnPropertyChanged(() => this.View);
                    this.OnPropertyChanged(() => this.Groups);
                }
            }
        }

        public event EventHandler<NotifyCollectionChangedEventArgs> SourceCollectionChanged;

        public ObservableCollection<T> Source
        {
            get
            {
                return this.sourceCollection;
            }
            set
            {
                // Remove previous collection changed event
                if (this.sourceCollection != null)
                {
                    this.sourceCollection.CollectionChanged -= this.HandleSourceCollectionChanged;
                }

                this.sourceCollection = value;

                // Subscribe to collection changed event
                if (this.sourceCollection != null)
                {
                    this.sourceCollection.CollectionChanged += this.HandleSourceCollectionChanged;
                }

                this.HandleSourceCollectionChanged(this.sourceCollection, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
        }

        private void HandleSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.Refresh();

            var handler = this.SourceCollectionChanged;
            if (handler != null)
            {
                handler(sender, e);
            }
        }

        public ObservableCollection<T> View
        {
            get
            {
                // View returns the original collection if no filtering, search and ordering is applied.
                // The order of processing is set-up in a way to guarantee maximum performance.

                var viewCollection = this.Source;
                if (viewCollection != null && viewCollection.Any())
                {
                    if (!string.IsNullOrEmpty(this.SearchText))
                    {
                        viewCollection = this.Search(viewCollection, this.SearchText);
                    }

                    if (this.filterHandler != null)
                    {
                        viewCollection = this.GetFilteredCollection(viewCollection);
                    }

                    if (this.orderSpecifications != null && this.orderSpecifications.Any())
                    {
                        viewCollection = PerformOrdering(viewCollection, this.orderSpecifications).ToObservableCollection();
                    }
                }

                // It is important to return the viewCollection in a new ObservableCollection object.
                // Otherwise the binding is not refreshed when OnPropertyChanged is called.
                return new ObservableCollection<T>(viewCollection);
            }
        }
        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Adds a new order specification for a certain property of given type T.
        /// </summary>
        /// <param name="keySelector">Lambda expression to select the ordering property.</param>
        /// <param name="orderDirection">Order direction in which the selected property shall be sorted.</param>
        public void AddOrderSpecification(Expression<Func<T, object>> keySelector, OrderDirection orderDirection = OrderDirection.Ascending)
        {
            this.AddOrderSpecificationInternal(keySelector, orderDirection);
            this.Refresh();
        }

        /// <inheritdoc />
        void IObservableView.AddOrderSpecification(string propertyName, OrderDirection orderDirection = OrderDirection.Ascending)
        {
            this.AddOrderSpecification(propertyName, orderDirection);
        }

        /// <inheritdoc />
        void IObservableView.RemoveOrderSpecification(string propertyName)
        {
            this.orderSpecifications.RemoveAll(x => x.PropertyName == propertyName);
        }

        public void AddOrderSpecification(string propertyName, OrderDirection orderDirection = OrderDirection.Ascending)
        {
            var parameter = Expression.Parameter(typeof(T));
            var memberExpression = Expression.Property(parameter, propertyName);
            var keySelector = Expression.Lambda<Func<T, object>>(memberExpression, parameter);

            this.AddOrderSpecificationInternal(keySelector, orderDirection);
        }

        private void AddOrderSpecificationInternal(Expression<Func<T, object>> keySelector, OrderDirection orderDirection)
        {
            if (keySelector == null)
            {
                throw new ArgumentNullException("keySelector");
            }
            var newOrderSpecification = new OrderSpecification<T>(keySelector, orderDirection);
            var index = this.orderSpecifications.FindIndex(x => x.PropertyName == newOrderSpecification.PropertyName);
            if (index > -1)
            {
                this.orderSpecifications[index] = newOrderSpecification;
            }
            else
            {
                this.orderSpecifications.Add(newOrderSpecification);
            }
        }

        /// <inheritdoc />
        OrderDirection? IObservableView.GetSortSpecification(string propertyName)
        {
            var orderSpecification = this.orderSpecifications.SingleOrDefault(s => s.PropertyName == propertyName);
            return orderSpecification != null ? orderSpecification.OrderDirection : (OrderDirection?)null;
        }

        /// <inheritdoc />
        void IObservableView.ClearOrderSpecifications()
        {
            this.orderSpecifications.Clear();
        }

        /// <inheritdoc />
        public void ClearOrderSpecifications()
        {
            this.orderSpecifications.Clear();

            this.Refresh();
        }

        /// <summary>
        ///     Refreshes the Source, View and Groups property of this instance.
        /// </summary>
        public void Refresh()
        {
            this.OnPropertyChanged(() => this.Source);
            this.OnPropertyChanged(() => this.View);
            this.OnPropertyChanged(() => this.Groups);
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Called when [property changed].
        /// </summary>
        /// <typeparam name="TX">The type of the tx.Generic type T.Generic type T.</typeparam>
        /// <param name="propertyExpression">The property expression.</param>
        /// <exception cref="ArgumentException">
        ///     'propertyExpression' should be a member expression
        ///     or
        ///     'propertyExpression' body should be a constant expression.
        /// </exception>
        protected virtual void OnPropertyChanged<TX>(Expression<Func<TX>> propertyExpression)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                var body = propertyExpression.Body as MemberExpression;
                if (body == null)
                {
                    throw new ArgumentException("'propertyExpression' should be a member expression");
                }

                var expression = body.Expression as ConstantExpression;
                if (expression == null)
                {
                    throw new ArgumentException("'propertyExpression' body should be a constant expression");
                }

                var e = new PropertyChangedEventArgs(body.Member.Name);
                handler(this, e);
            }
        }

        private Expression AddExpression(ParameterExpression parameterExpression, string propertyName, string value)
        {
            Expression returnExpression = null;
            Expression rightExpression = Expression.Constant(value.ToLower());

            var propertyInfo = typeof(T).GetRuntimeProperty(propertyName);
            if (propertyInfo != null)
            {
                Expression left = Expression.Property(parameterExpression, propertyInfo);
                if (left.Type == typeof(string))
                {
                    // If the given property is of type string, we want to compare them in lower letters.
                    Expression toLowerExpression = left.ToLower();
                    Expression removeDiacriticsExpression = Expression.Call(null, typeof(StringExtensions).GetRuntimeMethod("RemoveDiacritics", new[] { typeof(string) }), toLowerExpression);
                    Expression containsExpression = removeDiacriticsExpression.Contains(rightExpression);
                    returnExpression = Expression.OrElse(containsExpression, toLowerExpression.Contains(rightExpression)); // There are two comparisons done: One with diacritics and one without.
                }
                else if (left.Type == typeof(int))
                {
                    // If the given property is of type integer, we want to convert it to string first.
                    Expression leftToLower = Expression.Call(left, typeof(int).GetRuntimeMethod("ToString", new Type[] { })); // TODO: use ToLower extension method
                    returnExpression = leftToLower.Contains(rightExpression);
                }
                else if (left.Type.GetTypeInfo().IsEnum)
                {
                    // TODO: Handle enum localized strings
                }
            }

            return returnExpression;
        }

        ////private Expression CreateToLowerContainsExpression(Expression leftExpression, Expression rightExpression)
        ////{
        ////    Expression leftToLower = Expression.Call(leftExpression, typeof(string).GetRuntimeMethod("ToLower", new Type[] { }));
        ////    return Expression.Call(leftToLower, typeof(string).GetRuntimeMethod("Contains", new[] { typeof(string) }), rightExpression);
        ////}

        private ObservableCollection<T> GetFilteredCollection(IEnumerable<T> viewCollection)
        {
            var filteredCollection = new ObservableCollection<T>();
            foreach (T item in viewCollection)
            {
                var filterEventArgs = new FilterEventArgs<T>(item);
                this.filterHandler(this, filterEventArgs);

                if (filterEventArgs.IsAllowed)
                {
                    filteredCollection.Add(item);
                }
            }

            return filteredCollection;
        }

        public void AddSearchSpecification<TProperty>(Expression<Func<T, TProperty>> propertyExpression)
        {
            if (propertyExpression == null)
            {
                throw new ArgumentNullException("propertyExpression");
            }

            var propertyInfo = propertyExpression.GetPropertyInfo();
            var isAdded = this.searchSpecifications.Add(propertyInfo);
            if (isAdded == false)
            {
                throw new InvalidOperationException(string.Format("Could not add property {0}", propertyInfo.Name));
            }

            this.Refresh();
        }

        /// <summary>
        /// Removes all search specifications and resets <code>SearchText</code> to <code>string.Empty</code>.
        /// </summary>
        public void ClearSearchSpecifications()
        {
            this.SearchText = string.Empty;
            this.searchSpecifications.Clear();

            this.Refresh();
        }

        private IEnumerable<PropertyInfo> GetSearchableAttributes()
        {
            return typeof(T).GetRuntimeProperties().Where(propertyInfo =>
                propertyInfo.CustomAttributes.Any(attr =>
                    attr.AttributeType == typeof(SearchableAttribute))).ToList();
        }

        /// <summary>
        /// Performs a search operation using the given <param name="pattern">search pattern</param>.
        /// The search operation is performed on the properties <code>View</code> and <code>Group</code>. 
        /// </summary>
        /// <param name="pattern"></param>
        public void Search(string pattern)
        {
            this.SearchText = pattern;
        }

        public void ClearSearch()
        {
            this.Search(string.Empty);
        }

        private ObservableCollection<T> Search(IEnumerable<T> viewCollection, string pattern)
        {
            var results = new ObservableCollection<T>();

            if (string.IsNullOrEmpty(pattern))
            {
                return viewCollection.ToObservableCollection();
            }

            string[] searchStrings = pattern.Trim().Split(new[] { " ", "," }, StringSplitOptions.RemoveEmptyEntries);

            // TODO: Define more characters which can split the search termin into atomic words.
            if (!searchStrings.Any())
            {
                return results;
            }

            if (!this.searchSpecifications.Any())
            {
                throw new InvalidOperationException(
                    string.Format("Please add at least one search specification either by calling AddSearchSpecification"
                                  + " or by defining [Searchable] annotation in your type {0} to mark properties as searchable.",
                                  typeof(T).Name));
            }

            Expression baseExpression = null;
            ParameterExpression parameterExpression = Expression.Parameter(typeof(T), "x");

            foreach (string searchString in searchStrings)
            {
                Expression argumentBaseExpression = null;
                foreach (PropertyInfo propertyInfo in this.searchSpecifications)
                {
                    Expression nextExpression = this.AddExpression(parameterExpression, propertyInfo.Name, searchString);
                    if (nextExpression != null)
                    {
                        if (argumentBaseExpression == null)
                        {
                            argumentBaseExpression = nextExpression;
                        }
                        else
                        {
                            argumentBaseExpression = Expression.OrElse(argumentBaseExpression, nextExpression);
                        }
                    }
                }

                if (baseExpression == null)
                {
                    baseExpression = argumentBaseExpression;
                }
                else
                {
                    baseExpression = Expression.AndAlso(baseExpression, argumentBaseExpression);
                }
            }

            if (baseExpression == null)
            {
                return results;
            }

            IQueryable<T> queryableDtos = viewCollection.AsQueryable();

            MethodCallExpression whereCallExpression = Expression.Call(
                typeof(Queryable),
                "Where",
                new[] { queryableDtos.ElementType },
                queryableDtos.Expression,
                Expression.Lambda<Func<T, bool>>(baseExpression, new[] { parameterExpression }));

            return queryableDtos.Provider.CreateQuery<T>(whereCallExpression).ToObservableCollection();
        }

        private static IEnumerable<T> PerformOrdering(IEnumerable<T> enumerable, IEnumerable<OrderSpecification<T>> orderSpecifications)
        {
            IQueryable<T> query = enumerable.AsQueryable();

            OrderSpecification<T> firstSpecification = orderSpecifications.First();
            IOrderedEnumerable<T> orderedQuery;
            if (firstSpecification.OrderDirection == OrderDirection.Ascending)
            {
                orderedQuery = query.OrderBy(firstSpecification.KeySelector);
            }
            else
            {
                orderedQuery = query.OrderByDescending(firstSpecification.KeySelector);
            }

            foreach (var orderSpecification in orderSpecifications.Skip(1))
            {
                if (orderSpecification.OrderDirection == OrderDirection.Ascending)
                {
                    orderedQuery = orderedQuery.ThenBy(orderSpecification.KeySelector);
                }
                else
                {
                    orderedQuery = orderedQuery.ThenByDescending(orderSpecification.KeySelector);
                }
            }

            return orderedQuery.ToList();
        }

        #endregion
    }
}