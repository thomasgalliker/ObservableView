using ObservableView.Searching.Operands;

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
            constantOperand.Should().NotBeNull();
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
            constantOperand.Should().NotBeNull();
            constantOperand.Type.Should().Be<int>();
        }

        [Fact]
        public void ShouldBuildStringConstant()
        {
            // Arrange
            IExpressionBuilder expressionBuilder = new ExpressionBuilder(typeof(Car));

            var constantOperand = new ConstantOperand(CarPool.carVwGolf.Model);

            // Act
            var expression = constantOperand.Build(expressionBuilder);

            // Assert
            expression.Should().NotBeNull();
            expression.Type.Should().Be<string>();

            var lambda = Expression.Lambda<Func<string>>(expression);
            var func = lambda.Compile();
            var modelName = func();

            modelName.Should().Be(CarPool.carVwGolf.Model);
        }

        [Fact]
        public void ShouldReturnDefaultValueOfNullableInteger()
        {
            // Arrange
            IExpressionBuilder expressionBuilder = new ExpressionBuilder(typeof(Car));

            var constantOperand = new ConstantOperand(typeof(int?));

            // Act
            var expression = constantOperand.Build(expressionBuilder);

            // Assert
            constantOperand.Value.Should().BeNull();

            expression.Should().NotBeNull();
            expression.Type.Should().Be<int?>();

            var lambda = Expression.Lambda<Func<int?>>(expression);
            var func = lambda.Compile();
            var constantValue = func();

            constantValue.Should().Be(default(int?));
        }
    }
}