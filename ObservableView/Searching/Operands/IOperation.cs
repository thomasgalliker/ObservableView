using ObservableView.Searching.Operators;

namespace ObservableView.Searching.Operands
{
    public interface IOperation
    {
        public IOperator Operator { get; }

        Expression Build(ExpressionBuilder expressionBuilder);
    }
}