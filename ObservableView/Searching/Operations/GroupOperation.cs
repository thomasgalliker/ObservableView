using ObservableView.Searching.Operands;
using ObservableView.Searching.Operators;

namespace ObservableView.Searching.Operations
{
    // [DataContract(Name = "GroupOperation")]
    public class GroupOperation : Operation
    {
        public GroupOperation(Operation leftOperation, Operation rightOperation, GroupOperator groupOperator)
            : base(groupOperator)
        {
            this.LeftOperation = leftOperation;
            this.RightOperation = rightOperation;
        }

        // [DataMember(Name = "LeftOperation", IsRequired = true)]
        public Operation LeftOperation { get; set; }

        // [DataMember(Name = "RightOperation", IsRequired = true)]
        public Operation RightOperation { get; set; }
    }
}