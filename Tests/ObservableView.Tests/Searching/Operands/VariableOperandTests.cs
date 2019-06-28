using FluentAssertions;

using ObservableView.Searching.Operands;

using Xunit;

namespace ObservableView.Tests.Searching.Operands
{
    public class VariableOperandTests
    {
        [Fact]
        public void ShouldReturnCorrectVariableName()
        {
            // Arrange
            const string VariableName = "test";

            // Act
            var variableOperand = new VariableOperand(variableName: VariableName, type: typeof(string));

            // Assert
            variableOperand.Should().NotBeNull();
            variableOperand.VariableName.Should().Be(VariableName);
            variableOperand.Type.Should().Be<string>();
        }

        [Fact]
        public void ShouldHaveWritableValueProperty()
        {
            // Arrange
            const string Value = "test";
            var variableOperand = new VariableOperand(variableName: "test", type: typeof(string));

            // Act
            variableOperand.Value = Value;

            // Assert
            variableOperand.Should().NotBeNull();
            variableOperand.Value.Should().Be(Value);
            variableOperand.Type.Should().Be<string>();
        }
    }
}