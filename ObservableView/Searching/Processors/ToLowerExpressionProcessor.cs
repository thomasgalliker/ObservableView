using System.Diagnostics;

namespace ObservableView.Searching.Processors
{
    [DebuggerDisplay("ToLowerExpressionProcessor")]
    public class ToLowerExpressionProcessor : ExpressionProcessor
    {
        public override Expression Process(Expression expression)
        {
            Expression toLowerExpression = expression.ToLower();

            return toLowerExpression;
        }
    }
}