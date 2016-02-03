using FluentAssertions;

using ObservableView.Searching.Operands;

using Xunit;

namespace ObservableView.Tests.Searching.Operands
{
    public class ConstantOperandTests
    {
        [Fact]
        public void ShouldReturnCorrectType()
        {
            // Act
            var constantOperand = new ConstantOperand("test");

            // Assert
            constantOperand.Type.Should().Be<string>();
        }

        [Fact]
        public void ShouldReturnCorrectTypeWhenValueHasChanged()
        {
            // Arrange
            const string OriginalValue = "1234";
            const int ChangedValue = 1234;
            var constantOperand = new ConstantOperand(OriginalValue);

            // Act
            constantOperand.Value = ChangedValue;

            // Assert
            constantOperand.Type.Should().Be<int>();
        }
    }
}