using ObservableView.Searching.Operands;
using ObservableView.Searching.Operators;

namespace ObservableView.Searching.Operations
{
    // [DataContract(Name = "GroupOperation")]
    public class GroupOperation : Operation<GroupOperator>
    {
        public GroupOperation(IOperation leftOperation, IOperation rightOperation, GroupOperator groupOperator)
            : base(groupOperator)
        {
            this.LeftOperation = leftOperation;
            this.RightOperation = rightOperation;
        }

        // [DataMember(Name = "LeftOperation", IsRequired = true)]
        public IOperation LeftOperation { get; }

        // [DataMember(Name = "RightOperation", IsRequired = true)]
        public IOperation RightOperation { get; }
    }
}