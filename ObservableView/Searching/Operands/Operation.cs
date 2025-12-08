using ObservableView.Searching.Operators;

namespace ObservableView.Searching.Operands
{
    public abstract class Operation // TODO: Convert this class into an interface
    {
        protected Operation(IOperator @operator)
        {
            this.Operator = @operator;
        }

        // [DataMember(Name = "Operator", IsRequired = true)]
        public IOperator Operator { get; set; }
    }
}