using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;

using ObservableView.Extensions;

namespace ObservableView.Searching.Operands
{
    [DebuggerDisplay("ConstantOperand: Value={Value}, Type={Type}")]
    public class ConstantOperand : Operand
    {
        private object value;

        /// <summary>
        /// Takes a <param name="type">target type</param>. The value is not (yet) specified.
        /// </summary>
        public ConstantOperand(Type type)
        {
            this.Type = type;
        }

        /// <summary>
        /// Takes a <param name="value">value</param> of arbitrary data.
        /// </summary>
        public ConstantOperand(object value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            this.Value = value;
            this.Type = value.GetType();
        }

        /// <summary>
        /// Takes a <param name="value">value</param> of arbitrary data
        /// which will be converted to <param name="type">type</param> at build time.
        /// </summary>
        public ConstantOperand(object value, Type type)
            : this(value)
        {
            this.Type = type;
        }

        public Type Type { get; private set; }

        public object Value
        {
            get
            {
                return this.value;
            }
            set
            {
                this.value = value;
                this.Type = value == null ? typeof(object) : value.GetType();
            }
        }

        public override Expression Build(IExpressionBuilder expressionBuilder)
        {
            Expression constantExpression = null;

            var genericType = ReflectionHelper.GetGenericType(this.Type);
            object convertedValue = CheckAndConvertType(genericType, this.Value);

            if (convertedValue == null)
            {
                constantExpression = Expression.Default(this.Type);
            }
            else
            {
                constantExpression = Expression.Constant(convertedValue, this.Type);
            }

            return constantExpression;
        }

        private static object CheckAndConvertType(Type type, object value)
        {
            if (type != null && value != null)
            {
                try
                {
                    if (type.GetTypeInfo().IsEnum)
                    {
                        value = Enum.ToObject(type, value);
                    }
                    else
                    {
                        value = Convert.ChangeType(value, type, CultureInfo.InvariantCulture);
                    }
                }
                catch (FormatException)
                {
                    throw new FormatException(string.Format(CultureInfo.InvariantCulture, "Value {0} does not match type {1}.", value, type));
                }
            }

            return value;
        }
    }
}