namespace ObservableView.Tracking
{
    /// <summary>
    /// Provides data for the <see cref="ObservableView{T}.ItemPropertyChanged"/> event.
    /// </summary>
    public sealed class ItemPropertyChangedEventArgs<T> : EventArgs
    {
        public ItemPropertyChangedEventArgs(T item, string? propertyName)
        {
            this.Item = item;
            this.PropertyName = propertyName;
        }

        /// <summary>
        /// Gets the item whose property has changed.
        /// </summary>
        public T Item { get; }

        /// <summary>
        /// Gets the name of the property that changed on <see cref="Item"/>.
        /// </summary>
        public string? PropertyName { get; }
    }
}
