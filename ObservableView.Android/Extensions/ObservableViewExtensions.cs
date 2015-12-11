using System;
using System.Collections.Generic;

using Android.Views;

namespace ObservableView.Extensions
{
    public static class ObservableViewExtensions
    {
        /// <summary>
        /// Creates a new <see cref="ObservableAdapter{T}"/> for a given <see cref="IList{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the items contained in the <see cref="IList{T}"/>.</typeparam>
        /// <param name="list">The list that the adapter will be created for.</param>
        /// <param name="getTemplateDelegate">A method taking an item's position in the list, the item itself,
        /// and a recycled Android <see cref="View"/>, and returning an adapted View for this item. Note that the recycled
        /// View might be null, in which case a new View must be inflated by this method.</param>
        /// <param name="getHeaderTemplateDelegate"></param>
        /// <returns>An adapter adapted to the collection passed in parameter..</returns>
        public static ObservableViewAdapter<T> GetAdapter<T>(
            this ObservableView<T> list,
            Func<View, T, View> getTemplateDelegate,
            Func<View, Grouping<T>, View> getHeaderTemplateDelegate) where T : class
        {
            return new ObservableViewAdapter<T>
            {
                DataSource = list,
                GetTemplateDelegate = getTemplateDelegate,
                GetHeaderTemplateDelegate = getHeaderTemplateDelegate,
            };
        }

        public static ObservableViewAdapter<T> GetAdapter<T>(
            this ObservableView<T> list,
            Func<View, T, View> getTemplateDelegate) where T : class
        {
            return new ObservableViewAdapter<T>
            {
                DataSource = list,
                GetTemplateDelegate = getTemplateDelegate,
            };
        }
    }
}