using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

using Android.Views;
using Android.Widget;

namespace ObservableView
{
    public class ObservableViewAdapter<T> : BaseAdapter<T>, ISectionIndexer
        where T : class
    {
        private const int TYPE_ITEM = 0;
        private const int TYPE_SEPARATOR = 1;
        private ObservableView<T> dataSource;

        public ObservableView<T> DataSource
        {
            get
            {
                return this.dataSource;
            }
            set
            {
                if (Equals(this.dataSource, value))
                {
                    return;
                }

                if (this.dataSource != null)
                {
                    this.dataSource.SourceCollectionChanged -= this.HandleCollectionChanged;
                    this.dataSource.PropertyChanged -= this.HandlePropertyChanged;
                }

                this.dataSource = value;

                if (this.dataSource != null)
                {
                    this.dataSource.SourceCollectionChanged += this.HandleCollectionChanged;
                    this.dataSource.PropertyChanged += this.HandlePropertyChanged;
                }
            }
        }

        /// <summary>
        ///     Gets and sets a method taking an item's position in the list, the item itself,
        ///     and a recycled Android View, and returning an adapted View for this item. Note that the recycled
        ///     view might be null, in which case a new View must be inflated by this method.
        /// </summary>
        public Func<View, T, View> GetTemplateDelegate { get; set; }
        public Func<View, Grouping<T>, View> GetHeaderTemplateDelegate { get; set; }

        public override int Count
        {
            get
            {
                if (this.dataSource == null)
                {
                    return 0;
                }

                int count = this.dataSource.View.Count;
                count += this.dataSource.Groups.Count();

                return count;
            }
        }

        public override int ViewTypeCount
        {
            get
            {
                return 2;
            }
        }

        private static IEnumerable<object> FlattenGroupings(IEnumerable<Grouping<T>> groupings)
        {
            foreach (var grouping in groupings)
            {
                yield return grouping;
                foreach (var g in grouping)
                {
                    yield return g;
                }
            }
        }

        private object GetItemOrGroup(int position)
        {
            var flattenedGroups = FlattenGroupings(this.dataSource.Groups);

            using (var enumerator = flattenedGroups.GetEnumerator())
            {
                int index = 0;
                while (enumerator.MoveNext())
                {
                    if (index == position)
                    {
                        return enumerator.Current;
                    }
                    index++;
                }
            }

            return null;
        }

        public override int GetItemViewType(int position)
        {
            return this.GetItemOrGroup(position) is T ? TYPE_ITEM : TYPE_SEPARATOR;
        }

        public int GetPositionForSection(int section)
        {
            var groups = this.dataSource.Groups.ToList();
            if (section >= groups.Count)
            {
                return groups.Count - 1;
            }

            var character = groups[section];
            return section;
        }

        public int GetSectionForPosition(int position)
        {
            return 1;
        }

        public Java.Lang.Object[] GetSections()
        {
            var array = this.dataSource.Groups.Select(s => new Java.Lang.String(s.Key)).ToArray();
            return array;
        }

        /// <summary>
        ///     Gets the item corresponding to the index in the DataSource.
        /// </summary>
        /// <param name="index">The index of the item that needs to be returned.</param>
        /// <returns>The item corresponding to the index in the DataSource</returns>
        public override T this[int index]
        {
            get
            {
                return this.dataSource == null ? default(T) : this.dataSource.View[index];
            }
        }

        /// <summary>
        ///     Returns a unique ID for the item corresponding to the position parameter.
        ///     In this implementation, the method always returns the position itself.
        /// </summary>
        /// <param name="position">The position of the item for which the ID needs to be returned.</param>
        /// <returns>A unique ID for the item corresponding to the position parameter.</returns>
        public override long GetItemId(int position)
        {
            return position;
        }

        /// <summary>
        ///     Prepares the view (template) for the item corresponding to the position
        ///     in the DataSource. This method calls the <see cref="GetTemplateDelegate" /> method so that the caller
        ///     can create (if necessary) and adapt the template for the corresponding item.
        /// </summary>
        /// <param name="position">The position of the item in the DataSource.</param>
        /// <param name="convertView">
        ///     A recycled view. If this parameter is null,
        ///     a new view must be inflated.
        /// </param>
        /// <param name="parent">The view's parent.</param>
        /// <returns>A view adapted for the item at the corresponding position.</returns>
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = null;
            object itemOrGroup = this.GetItemOrGroup(position);

            var item = itemOrGroup as T;
            if (item != null)
            {
                if (this.GetTemplateDelegate == null)
                {
                    return convertView;
                }

                view = this.GetTemplateDelegate(convertView, item); 
            }
            else
            {
                if (this.GetHeaderTemplateDelegate == null)
                {
                    return convertView;
                }

                var group = itemOrGroup as Grouping<T>;
                if (group == null)
                {
                    throw new ArgumentNullException("group");
                }

                view = this.GetHeaderTemplateDelegate(convertView, group);
            }

            return view;
        }

        private void HandleCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.NotifyDataSetChanged();
        }

        private void HandlePropertyChanged(object sender, PropertyChangedEventArgs e)
        {

            // TODO GATH "View" and "Groups" would be enough!?
            if (/*e.PropertyName == "SearchText" || */e.PropertyName == "View" || e.PropertyName == "Groups")
            {
                this.NotifyDataSetChanged();
            }
        }

        public void Refresh() //TODO GATH: Really needed???
        {
            this.dataSource.Refresh();
        }
    }
}