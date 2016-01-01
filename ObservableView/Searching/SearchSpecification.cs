using System;
using System.Linq;
using System.Linq.Expressions;

using ObservableView.Extensions;
using ObservableView.Filtering;
using ObservableView.Searching.Operands;
using ObservableView.Searching.Operations;
using ObservableView.Searching.Operators;

namespace ObservableView.Searching
{
    public class SearchSpecification<T> : ISearchSpecification<T>
    {
        private static readonly VariableOperand DefaultSearchTextVariableOperand = new VariableOperand("searchText");

        public event EventHandler SearchSpecificationAdded;

        public event EventHandler SearchSpecificationsCleared;

        public Operation BaseOperation { get; private set; }

        public ISearchSpecification<T> Add<TProperty>(Expression<Func<T, TProperty>> propertyExpression, BinaryOperator @operator) // TODO: BinaryOperator @operator = null then take default depending on property type
        {
            if (propertyExpression == null)
            {
                throw new ArgumentNullException("propertyExpression");
            }

            if (this.BaseOperation == null)
            {
                var propertyInfo = ReflectionHelper<T>.GetProperty(propertyExpression);
                this.BaseOperation = new BinaryOperation(@operator, new PropertyOperand(propertyInfo), DefaultSearchTextVariableOperand);
            }
            else
            {
                return this.Or(propertyExpression, @operator);
            }

            this.OnSearchSpecificationAdded();
            return this;
        }

        public ISearchSpecification<T> And<TProperty>(Expression<Func<T, TProperty>> propertyExpression, BinaryOperator @operator)
        {
            return this.CreateNestedOperation(propertyExpression, @operator, GroupOperator.And);
        }

        public ISearchSpecification<T> Or<TProperty>(Expression<Func<T, TProperty>> propertyExpression, BinaryOperator @operator)
        {
            return this.CreateNestedOperation(propertyExpression, @operator, GroupOperator.Or);
        }

        private ISearchSpecification<T> CreateNestedOperation<TProperty>(Expression<Func<T, TProperty>> propertyExpression, BinaryOperator @operator, GroupOperator groupOperator)
        {
            if (this.BaseOperation == null)
            {
                throw new InvalidOperationException("Call Add beforehand.");
            }

            var propertyInfo = ReflectionHelper<T>.GetProperty(propertyExpression);

            var nestedBinaryOperation = new BinaryOperation(@operator, new PropertyOperand(propertyInfo), DefaultSearchTextVariableOperand);

            this.BaseOperation = new GroupOperation(this.BaseOperation, nestedBinaryOperation, groupOperator);

            this.OnSearchSpecificationAdded();
            return this;
        }

        private void OnSearchSpecificationAdded()
        {
            var handler = this.SearchSpecificationAdded;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        public void ReplaceSearchTextVariables<TX>(TX value)
        {
            this.ReplaceVariables(DefaultSearchTextVariableOperand.VariableName, value);
        }

        public void ReplaceVariables<TX>(string variableName, TX value)
        {
            var variableOperands = this.BaseOperation.Flatten().OfType<VariableOperand>().Where(v => v.VariableName == variableName);

            foreach (var variableOperand in variableOperands)
            {
                variableOperand.Value = value;
            }
        }

        public void Clear()
        {
            this.BaseOperation = null;
            this.OnSearchSpecificationsCleared();
        }

        private void OnSearchSpecificationsCleared()
        {
            var handler = this.SearchSpecificationsCleared;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        public bool Any()
        {
            return this.BaseOperation != null;
        }
    }
}