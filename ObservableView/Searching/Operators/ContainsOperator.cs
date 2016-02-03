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
        public override Expression Build(IExpressionBuilder expressionBuilder, Operation operation)
        {
            BinaryOperation binaryOperation = (BinaryOperation)operation;

            var leftExpression = expressionBuilder.Build(binaryOperation.LeftOperand);

            MethodInfo containsMethodInfo = ReflectionHelper<string>.GetMethod<string>((source, argument) => source.Contains(argument));

            var rightExpression = expressionBuilder.Build(binaryOperation.RightOperand);
            if (rightExpression.Type != typeof(string))
            {
                rightExpression = rightExpression.ToStringExpression();
            }

            Expression containsExpression = Expression.Call(
                leftExpression,
                containsMethodInfo,
                rightExpression);

            return containsExpression;
        }
    }
}