namespace ObservableView.Searching.Operands
{
    public interface IOperand
    {
        Expression Build(IExpressionBuilder expressionBuilder);
    }
}