using System;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;

using ObservableView.Extensions;
using ObservableView.Searching.Operands;
using ObservableView.Searching.Operations;

namespace ObservableView.Searching.Operators
{
    [DebuggerDisplay("ContainsOperator")]
    public class ContainsOperator : BinaryOperator
    {
        private readonly StringComparison stringComparison;

        public ContainsOperator(StringComparison stringComparison = StringComparison.OrdinalIgnoreCase)
        {
            this.stringComparison = stringComparison;
        }

        public override Expression Build(IExpressionBuilder expressionBuilder, Operation operation)
        {
            BinaryOperation binaryOperation = (BinaryOperation)operation;  // TODO : Move to BinaryOperator

            var leftExpression = expressionBuilder.Build(binaryOperation.LeftOperand);
            if (leftExpression.Type != typeof(string))
            {
                leftExpression = leftExpression.ToStringExpression();
            }

            var rightExpression = expressionBuilder.Build(binaryOperation.RightOperand);
            if (rightExpression.Type != typeof(string))
            {
                rightExpression = rightExpression.ToStringExpression();
            }

            MethodInfo containsMethodInfo = ReflectionHelper<string>.GetMethod<string>((source, argument) => source.Contains(argument, this.stringComparison));
            var stringComparisonExpression = Expression.Constant(this.stringComparison);

            Expression containsExpression = Expression.Call(
                null, 
                containsMethodInfo,
                leftExpression, 
                rightExpression,
                stringComparisonExpression);

            return containsExpression;
        }
    }
}