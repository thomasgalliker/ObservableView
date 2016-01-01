using System;
using System.Linq.Expressions;

using ObservableView.Searching.Operands;
using ObservableView.Searching.Operators;

namespace ObservableView.Searching
{
    public interface ISearchSpecification<T>
    {
        event EventHandler SearchSpecificationAdded;

        event EventHandler SearchSpecificationsCleared;

        ISearchSpecification<T> Add<TProperty>(Expression<Func<T, TProperty>> propertyExpression, BinaryOperator @operator);

        ISearchSpecification<T> And<TProperty>(Expression<Func<T, TProperty>> propertyExpression, BinaryOperator @operator);

        ISearchSpecification<T> Or<TProperty>(Expression<Func<T, TProperty>> propertyExpression, BinaryOperator @operator);

        void ReplaceSearchTextVariables<TX>(TX value);

        Operation BaseOperation { get; }

        /// <summary>
        /// Removes all search specifications and resets <code>SearchText</code> to <code>string.Empty</code>.
        /// </summary>
        void Clear();

        /// <summary>
        /// Checks if there are any search specifications defined.
        /// </summary>
        /// <returns></returns>
        bool Any();
    }
}