using System.Diagnostics;

namespace ObservableView.Searching.Processors
{
    [DebuggerDisplay("TrimExpressionProcessor")]
    public class TrimExpressionProcessor : ExpressionProcessor
    {
        public override Expression Process(Expression expression)
        {
            Expression trimExpression = expression.Trim();

            return trimExpression;
        }
    }
}