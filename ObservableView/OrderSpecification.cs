using System;

namespace ObservableView
{
    /// <summary>
    ///     The order specification.
    /// </summary>
    /// <typeparam name="T">
    /// </typeparam>
    internal class OrderSpecification<T> // TODO GATH: Could be a struct?
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="OrderSpecification{T}" /> class.
        /// </summary>
        /// <param name="keySelector">The key selector.</param>
        /// <param name="orderDirection">The order direction.</param>
        public OrderSpecification(Func<T, object> keySelector, OrderDirection orderDirection)
        {
            this.KeySelector = keySelector;
            this.OrderDirection = orderDirection;
        }

        /// <summary>
        ///     Gets the key selector.
        /// </summary>
        /// <value>The key selector.</value>
        public Func<T, object> KeySelector { get; private set; }

        /// <summary>
        ///     Gets the order direction.
        /// </summary>
        /// <value>The order direction.</value>
        public OrderDirection OrderDirection { get; private set; }
    }
}