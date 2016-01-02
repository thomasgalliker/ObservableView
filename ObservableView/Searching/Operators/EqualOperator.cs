using System.Diagnostics;
using System.Linq.Expressions;
using System.Runtime.Serialization;

using ObservableView.Searching.Operands;
using ObservableView.Searching.Operations;

namespace ObservableView.Searching.Operators
{
    [DataContract(Name = "EqualOperator")]
    [DebuggerDisplay("EqualOperator")]
    public class EqualOperator : BinaryOperator
    {
        public override Expression Build(IExpressionBuilder expressionBuilder, Operation operation)
        {
            BinaryOperation binaryOperation = (BinaryOperation)operation;

            if (ExpressionBuilder.CheckIfSourceToTargetTypeMismatch(binaryOperation.LeftOperand, binaryOperation.RightOperand))
            {
                return Expression.Equal(Expression.Constant(true), Expression.Constant(false));
            }

            Expression equalExpression = Expression.Equal(expressionBuilder.Build(binaryOperation.LeftOperand), expressionBuilder.Build(binaryOperation.RightOperand));
            return equalExpression;
        }
    }
}