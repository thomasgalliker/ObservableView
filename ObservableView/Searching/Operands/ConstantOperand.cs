using System.Diagnostics;
using System.Linq.Expressions;

namespace ObservableView.Searching.Operands
{
    [DebuggerDisplay("ConstantOperand: Value={Value}")]
    public class ConstantOperand : IOperand
    {
        public ConstantOperand(object value)
        {
            this.Value = value;
        }

        public object Value { get; set; }

        public Expression Build(IExpressionBuilder expressionBuilder)
        {
            return Expression.Constant(this.Value, this.Value.GetType());
        }
    }
}