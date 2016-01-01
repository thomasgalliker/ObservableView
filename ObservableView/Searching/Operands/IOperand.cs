using System.Linq.Expressions;

namespace ObservableView.Searching.Operands
{
    public interface IOperand
    {
        Expression Build(IExpressionBuilder expressionBuilder);
    }
}