using ObservableView.Searching.Operands;
using ObservableView.Searching.Operations;

namespace ObservableView.Tests.Searching.Operators
{
    public class EqualOperatorTests
    {
        [Fact]
        public void ShouldCompareStringPropertyWithEqual()
        {
            // Arrange
            IExpressionBuilder expressionBuilder = new ExpressionBuilder(typeof(Car));

            var propertyInfo = ReflectionHelper<Car>.GetProperty(c => c.Model);
            PropertyOperand propertyOperand = new PropertyOperand(propertyInfo, expressionProcessors: null);
            string constantModelValue = CarPool.carAudiA1.Model;
            IOperand constantOperand = new ConstantOperand(value: constantModelValue);

            // Act
            BinaryOperator equalOperator = new EqualOperator();
            var binaryOperation = new BinaryOperation(equalOperator, propertyOperand, constantOperand);
            var expression = equalOperator.Build(expressionBuilder, binaryOperation);

            // Assert
            expression.Should().NotBeNull();
            expression.Type.Should().Be<bool>();

            var queryResult = TestHelper.ApplyExpression(CarPool.GetDefaultCarsList(), expression, expressionBuilder.ParameterExpression);
            queryResult.Should().HaveCount(1);
            queryResult.Should().Contain(CarPool.carAudiA1);
        }

        [Fact]
        public void ShouldCompareIntegerPropertyWithEqual()
        {
            // Arrange
            IExpressionBuilder expressionBuilder = new ExpressionBuilder(typeof(Car));

            var propertyInfo = ReflectionHelper<Car>.GetProperty(c => c.Year);
            PropertyOperand propertyOperand = new PropertyOperand(propertyInfo, expressionProcessors: null);
            const int ConstantYearValue = 2012;
            IOperand constantOperand = new ConstantOperand(value: ConstantYearValue);

            // Act
            BinaryOperator equalOperator = new EqualOperator();
            var binaryOperation = new BinaryOperation(equalOperator, propertyOperand, constantOperand);
            var expression = equalOperator.Build(expressionBuilder, binaryOperation);

            // Assert
            expression.Should().NotBeNull();
            expression.Type.Should().Be<bool>();

            var queryResult = TestHelper.ApplyExpression(CarPool.GetDefaultCarsList(), expression, expressionBuilder.ParameterExpression);
            queryResult.Should().HaveCount(1);
            queryResult.Should().Contain(CarPool.carBmwM3);
        }

        [Fact]
        public void ShouldCompareReferenceTypePropertyWithEqual()
        {
            // Arrange
            IExpressionBuilder expressionBuilder = new ExpressionBuilder(typeof(Car));

            var propertyInfo = ReflectionHelper<Car>.GetProperty(c => c.Engine);
            PropertyOperand propertyOperand = new PropertyOperand(propertyInfo, expressionProcessors: null);
            IOperand constantOperand = new ConstantOperand(value: Engines.electricEngine);

            // Act
            BinaryOperator equalOperator = new EqualOperator();
            var binaryOperation = new BinaryOperation(equalOperator, propertyOperand, constantOperand);
            var expression = equalOperator.Build(expressionBuilder, binaryOperation);

            // Assert
            expression.Should().NotBeNull();
            expression.Type.Should().Be<bool>();

            var queryResult = TestHelper.ApplyExpression(CarPool.GetDefaultCarsList(), expression, expressionBuilder.ParameterExpression);
            queryResult.Should().HaveCount(1);
            queryResult.Should().Contain(CarPool.carVwGolf);
        }

        [Fact]
        public void ShouldCompareNullReferenceTypePropertyWithEqual()
        {
            // Arrange
            IExpressionBuilder expressionBuilder = new ExpressionBuilder(typeof(Car));

            var propertyInfo = ReflectionHelper<Car>.GetProperty(c => c.Engine);
            PropertyOperand propertyOperand = new PropertyOperand(propertyInfo, expressionProcessors: null);
            IOperand constantOperand = new ConstantOperand(type: typeof(Engine));

            // Act
            BinaryOperator equalOperator = new EqualOperator();
            var binaryOperation = new BinaryOperation(equalOperator, propertyOperand, constantOperand);
            var expression = equalOperator.Build(expressionBuilder, binaryOperation);

            // Assert
            expression.Should().NotBeNull();
            expression.Type.Should().Be<bool>();

            var queryResult = TestHelper.ApplyExpression(CarPool.GetDefaultCarsList(), expression, expressionBuilder.ParameterExpression);
            queryResult.Should().HaveCount(1);
            queryResult.Should().Contain(CarPool.carAudiA4);
        }

        [Fact]
        public void ShouldThrowInvalidOperationExceptionIfSourceAndTargetTypeMismatch()
        {
            // Arrange
            IExpressionBuilder expressionBuilder = new ExpressionBuilder(typeof(Car));

            var propertyInfo = ReflectionHelper<Car>.GetProperty(c => c.Year);
            PropertyOperand propertyOperand = new PropertyOperand(propertyInfo, expressionProcessors: null);
            const string ConstantYearValue = "2012";
            IOperand constantOperand = new ConstantOperand(value: ConstantYearValue);

            // Act
            BinaryOperator equalOperator = new EqualOperator();
            var binaryOperation = new BinaryOperation(equalOperator, propertyOperand, constantOperand);
            Action action = () => equalOperator.Build(expressionBuilder, binaryOperation);

            // Assert
            action.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void ShouldNotThrowIfConstantOperandIsConvertedToPropertyType()
        {
            // Arrange
            IExpressionBuilder expressionBuilder = new ExpressionBuilder(typeof(Car));

            var propertyInfo = ReflectionHelper<Car>.GetProperty(c => c.Year);
            PropertyOperand propertyOperand = new PropertyOperand(propertyInfo, expressionProcessors: null);
            const string ConstantYearValue = "2012";
            IOperand constantOperand = new ConstantOperand(value: ConstantYearValue, type: propertyInfo.PropertyType);

            // Act
            BinaryOperator equalOperator = new EqualOperator();
            var binaryOperation = new BinaryOperation(equalOperator, propertyOperand, constantOperand);
            var expression = equalOperator.Build(expressionBuilder, binaryOperation);

            // Assert
            expression.Should().NotBeNull();
            expression.Type.Should().Be<bool>();

            var queryResult = TestHelper.ApplyExpression(CarPool.GetDefaultCarsList(), expression, expressionBuilder.ParameterExpression);
            queryResult.Should().HaveCount(1);
            queryResult.Should().Contain(CarPool.carBmwM3);
        }
    }
}
