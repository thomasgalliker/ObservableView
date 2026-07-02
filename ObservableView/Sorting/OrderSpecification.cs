namespace ObservableView.Sorting
{
    internal class OrderSpecification<T> // TODO GATH: Could be a struct?
    {
        public OrderSpecification(Expression<Func<T, object?>> keySelector, OrderDirection orderDirection)
        {
            if (keySelector == null)
            {
                throw new ArgumentNullException(nameof(keySelector));
            }

            this.KeySelector = keySelector.Compile();

            var propertyInfo = ReflectionHelper<T>.GetProperty(keySelector);
            if (propertyInfo == null)
            {
                throw new InvalidOperationException($"The property for the given expression could not be found (parameter: {nameof(keySelector)})");
            }

            this.PropertyName = propertyInfo.Name;
            this.OrderDirection = orderDirection;
        }

        public string PropertyName { get; private set; }

        public Func<T, object?> KeySelector { get; private set; }

        public OrderDirection OrderDirection { get; private set; }
    }
}