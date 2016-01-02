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
            MethodInfo contains = ReflectionHelper<string>.GetMethod<string>((source, argument) => source.Contains(argument));
            Expression containsExpression = Expression.Call(expressionBuilder.Build(binaryOperation.LeftOperand), contains, expressionBuilder.Build(binaryOperation.RightOperand));

            return containsExpression;
        }
    }
}