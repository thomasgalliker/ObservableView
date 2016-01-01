using System.Diagnostics;

namespace ObservableView.Searching.Operands
{
    [DebuggerDisplay("VariableOperand: VariableName={VariableName}, Value={Value}")]
    public class VariableOperand : ConstantOperand
    {
        public VariableOperand(string variableName)
            : base(null)
        {
            this.VariableName = variableName;
        }

        public string VariableName { get; set; }
    }
}