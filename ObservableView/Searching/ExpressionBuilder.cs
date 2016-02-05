using System;
using System.Linq.Expressions;
using System.Reflection;

using ObservableView.Extensions;
using ObservableView.Searching.Operands;

namespace ObservableView.Searching
{
    public class ExpressionBuilder : IExpressionBuilder
    {
        /// <summary>
        /// Creates a new instance of ExpressionBuilder.
        /// </summary>
        /// <param name="parameterExpression">Represents a named parameter expression.</param>
        public ExpressionBuilder(ParameterExpression parameterExpression)
        {
            this.ParameterExpression = parameterExpression;
        }

        public ParameterExpression ParameterExpression { get; }

        public Expression Build(Operation operation)
        {
            return operation.Operator.Build(this, operation);
        }

        public Expression Build(IOperand operand)
        {
            return operand.Build(this);
        }
    }
}