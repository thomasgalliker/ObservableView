using System.Diagnostics;

namespace ObservableView.Searching.Processors
{
    [DebuggerDisplay("ToUpperExpressionProcessor")]
    public class ToUpperExpressionProcessor : ExpressionProcessor
    {
        public override Expression Process(Expression expression)
        {
            Expression toLowerExpression = expression.ToUpper();

            return toLowerExpression;
        }
    }
}