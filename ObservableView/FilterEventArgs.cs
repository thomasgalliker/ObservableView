using System;

namespace ObservableView
{
    /// <summary>
    ///     The filter event args.
    /// </summary>
    /// <typeparam name="T">
    /// </typeparam>
    public class FilterEventArgs<T> : EventArgs
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="FilterEventArgs{T}" /> class.
        /// </summary>
        /// <param name="item">
        ///     The item.
        /// </param>
        public FilterEventArgs(T item)
        {
            this.Item = item;
            this.IsAllowed = true;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is allowed
        ///     Allowed means this item is included in the view returned in ObservableView.View property.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is allowed; otherwise, <c>false</c>.
        /// </value>
        public bool IsAllowed { get; set; }

        /// <summary>
        ///     Gets the current item of the collection.
        /// </summary>
        /// <value>
        ///     The item.
        /// </value>
        public T Item { get; private set; }

        #endregion
    }
}