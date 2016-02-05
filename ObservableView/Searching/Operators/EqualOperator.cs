using System;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Serialization;

using ObservableView.Extensions;
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
            BinaryOperation binaryOperation = (BinaryOperation)operation; // TODO : Move to BinaryOperator

            Expression leftExpression = expressionBuilder.Build(binaryOperation.LeftOperand);
            Expression rightExpression = expressionBuilder.Build(binaryOperation.RightOperand);

            Expression equalExpression = Expression.Equal(leftExpression, rightExpression);
            return equalExpression;
        }
    }
}