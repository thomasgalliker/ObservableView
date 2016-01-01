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

        /// <summary>
        /// Checks if the type of the left openad mismatches the type of the right operand.
        /// </summary>
        /// <returns></returns>
        public static bool CheckIfSourceToTargetTypeMismatch(IOperand left, IOperand right) // TODO GATH: Wrong place for this code
        {
            bool typeMismatch = false;

            var propertyOperand = left as PropertyOperand;
            var variableOperand = right as VariableOperand;

            if (propertyOperand != null && variableOperand != null)
            {
                Type propertyType = propertyOperand.PropertyInfo.PropertyType;

                if (propertyType != null && 
                    propertyType.GetTypeInfo().IsValueType &&
                    !ReflectionHelper.IsNullable(propertyType) &&
                    variableOperand.Value == null)
                {
                    typeMismatch = true;
                }
            }
            return typeMismatch;
        }
    }
}