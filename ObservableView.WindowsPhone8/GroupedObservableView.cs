using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

using Microsoft.Phone.Globalization;

namespace ObservableView
{
    /// <summary>
    /// The group.
    /// </summary>
    /// <typeparam name="T">
    /// </typeparam>
    public class Group<T> : List<T>
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="Group{T}" /> class.
        /// Public constructor.
        /// </summary>
        /// <param name="key">The key for this group.</param>
        public Group(string key)
        {
            this.Key = key;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Group{T}" /> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="list">The list.</param>
        public Group(string key, IEnumerable<T> list)
            : base(list)
        {
            this.Key = key;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// The Key of this group.
        /// </summary>
        /// <value>The key.</value>
        public string Key { get; private set; }

        #endregion
    }

    /// <summary>
    /// The grouped observable view.
    /// </summary>
    /// <typeparam name="T">Generic type T.</typeparam>
    public class GroupedObservableView<T> : ObservableView<T>
    {


        private static CultureInfo ci = CultureInfo.CurrentCulture;
        private static SortedLocaleGrouping slg = new SortedLocaleGrouping(ci);




        private Func<T, string> groupKey;



        /// <summary>
        /// Gets or sets the group key.
        /// </summary>
        public Func<T, string> GroupKey
        {
            get
            {
                return this.groupKey;
            }
            set
            {
                this.groupKey = value;
                this.OnPropertyChanged(() => this.GroupKey);

                this.OnPropertyChanged(() => this.Groups);

            }
        }

        /// <summary>
        /// Gets the groups.
        /// </summary>
        public List<Group<T>> Groups
        {
            get
            {
                ////return GetItemGroups(this.Source, this.GroupKey);
                List<Group<T>> list = CreateGroups(slg);
                foreach (T item in this.View)
                {
                    int index = 0;
                    if (slg.SupportsPhonetics)
                    {
                        // check if your database has yomi string for item
                        // if it does not, then do you want to generate Yomi or ask the user for this item.
                        // index = slg.GetGroupIndex(getKey(Yomiof(item)));
                    }
                    else
                    {
                        index = slg.GetGroupIndex(this.GroupKey(item));
                    }
                    if (index >= 0 && index < list.Count)
                    {
                        list[index].Add(item);
                    }
                }

                // Groups always use alphabetic sorting - no matter what OrderSpecification is defined.
                foreach (var group in list)
                {
                    group.Sort((c0, c1) => ci.CompareInfo.Compare(this.GroupKey(c0), this.GroupKey(c1)));
                }

                return list;
            }
        }

        /// <summary>
        /// Gets or sets the search text.
        /// </summary>
        public new string SearchText
        {
            get
            {
                return base.SearchText;
            }
            set
            {
                base.SearchText = value;

                this.OnPropertyChanged(() => this.Groups);

            }
        }

        /// <summary>
        /// Refreshes the Source, View and Groups property of this instance.
        /// </summary>
        public new void Refresh()
        {
            base.Refresh();

            this.OnPropertyChanged(() => this.Groups);

        }


        /// <summary>
        /// Creates the groups.
        /// </summary>
        /// <param name="soretedLocaleGrouping">The soreted locale grouping.</param>
        /// <returns>The List&lt;Group&lt;T&gt;&gt;.</returns>
        private static List<Group<T>> CreateGroups(SortedLocaleGrouping soretedLocaleGrouping)
        {
            return soretedLocaleGrouping.GroupDisplayNames.Select(key => new Group<T>(key)).ToList();
        }


    }
}