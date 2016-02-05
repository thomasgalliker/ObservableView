﻿using System.Diagnostics;
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

            Expression leftExpression = expressionBuilder.Build(binaryOperation.LeftOperand);
            Expression rightExpression = expressionBuilder.Build(binaryOperation.RightOperand);

            Expression equalExpression = Expression.Equal(leftExpression, rightExpression);
            return equalExpression;
        }
    }
}