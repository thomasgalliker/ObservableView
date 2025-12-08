using ObservableView.Searching.Operands;
using ObservableView.Searching.Operators;

namespace ObservableView.Searching.Operations
{
    public abstract class Operation<TOperator> : IOperation where TOperator : IOperator
    {
        protected Operation(TOperator @operator)
        {
            this.Operator = @operator;
        }

        public TOperator Operator { get; }

        IOperator IOperation.Operator => this.Operator;

        public Expression Build(ExpressionBuilder expressionBuilder)
        {
            return this.Operator.Build(expressionBuilder, this);
        }
    }
}