using System.Diagnostics;

using ObservableView.Searching.Processors;

namespace ObservableView.Searching.Operands
{
    // [DataContract(Name = "PropertyOperand")]
    [DebuggerDisplay("PropertyOperand: Name={PropertyInfo.Name}, Type={PropertyInfo.PropertyType.Name}")]
    public class PropertyOperand : Operand
    {
        private IExpressionProcessor[] expressionProcessors = Array.Empty<IExpressionProcessor>();

        public PropertyOperand(PropertyInfo propertyInfo, IExpressionProcessor[] expressionProcessors)
            : this(propertyInfo)
        {
            this.ExpressionProcessors = expressionProcessors ?? throw new ArgumentNullException(nameof(expressionProcessors));
        }

        public PropertyOperand(PropertyInfo propertyInfo)
        {
            this.PropertyInfo = propertyInfo;
        }

        // [DataMember(Name = "PropertyInfo", IsRequired = true)]
        public PropertyInfo PropertyInfo { get; set; }

        public IExpressionProcessor[] ExpressionProcessors
        {
            get => this.expressionProcessors;
            set => this.expressionProcessors = value ?? throw new ArgumentNullException(nameof(this.ExpressionProcessors));
        }

        public override Expression Build(IExpressionBuilder expressionBuilder)
        {
            Expression propertyExpression = Expression.Property(expressionBuilder.ParameterExpression, this.PropertyInfo);

            foreach (var expressionProcessor in this.ExpressionProcessors)
            {
                propertyExpression = expressionProcessor.Process(propertyExpression);
            }

            return propertyExpression;
        }
    }
}