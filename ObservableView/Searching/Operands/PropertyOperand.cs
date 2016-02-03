using System;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Serialization;

using ObservableView.Extensions;

namespace ObservableView.Searching.Operands
{
    [DataContract(Name = "PropertyOperand")]
    [DebuggerDisplay("PropertyOperand: Name={PropertyInfo.Name}")]
    public class PropertyOperand : IOperand
    {
        public PropertyOperand(PropertyInfo propertyInfo)
        {
            this.PropertyInfo = propertyInfo;
        }

        [DataMember(Name = "PropertyInfo", IsRequired = true)]
        public PropertyInfo PropertyInfo { get; set; }

        public Expression Build(IExpressionBuilder expressionBuilder)
        {
            Expression returnExpression = null;
            Expression propertyExpression = Expression.Property(expressionBuilder.ParameterExpression, this.PropertyInfo);

            if (propertyExpression.Type == typeof(string))
            {
                // If the given property is of type string, we want to compare them in lower letters.
                Expression toLowerExpression = propertyExpression.ToLower();
                returnExpression = toLowerExpression;
            }
            else if (propertyExpression.Type == typeof(int))
            {
                // If the given property is of type integer, we want to convert it to string first.
                Expression leftToLower = propertyExpression.ToStringExpression();
                returnExpression = leftToLower;
            }
            else if (propertyExpression.Type.GetTypeInfo().IsEnum)
            {
                // TODO: Handle enum localized strings
                throw new NotImplementedException("Handing of enums is not yet implemented.");
            }

            return returnExpression;
        }
    }
}