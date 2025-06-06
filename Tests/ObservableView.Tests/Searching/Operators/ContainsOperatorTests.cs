﻿using ObservableView.Searching.Operands;
using ObservableView.Searching.Operations;

namespace ObservableView.Tests.Searching.Operators
{
    public class ContainsOperatorTests
    {
        /// <summary>
        /// This test shall show how ExpressionBuilder behaves, when
        /// 1) a Contains operation which shall only be used for strings
        /// 2) is performed on an Int32 property
        /// 3) which does not contain a valid Int32 value (=null)
        /// </summary>
        [Fact]
        public void ShouldAllowContainsOperatorWithIntegerValues()
        {
            // Arrange
            IExpressionBuilder expressionBuilder = new ExpressionBuilder(typeof(Car));

            var propertyInfo = ReflectionHelper<Car>.GetProperty(x => x.Year);
            var propertyOperand = new PropertyOperand(propertyInfo);
            var constantOperand = new ConstantOperand("20");

            var containsOperator = new ContainsOperator();
            var binaryOperation = new BinaryOperation(BinaryOperator.Contains, propertyOperand, constantOperand);

            // Act
            var containsExpression = containsOperator.Build(expressionBuilder, binaryOperation);

            // Assert
            containsExpression.Should().NotBeNull();
            containsExpression.Type.Should().Be<bool>();

            var queryResult = TestHelper.ApplyExpression(CarPool.GetDefaultCarsList(), containsExpression, expressionBuilder.ParameterExpression);
            queryResult.Should().HaveCount(5); // All built in year 20xx
            queryResult.Should().Contain(CarPool.carAudiA1);
            queryResult.Should().Contain(CarPool.carAudiA3);
            queryResult.Should().Contain(CarPool.carAudiA4);
            queryResult.Should().Contain(CarPool.carBmwM1);
            queryResult.Should().Contain(CarPool.carBmwM3);
        }

        [Fact]
        public void ShouldRespectStringComparisonOrdinal()
        {
            // Arrange
            IExpressionBuilder expressionBuilder = new ExpressionBuilder(typeof(Car));

            var propertyOperandModel = new PropertyOperand(ReflectionHelper<Car>.GetProperty(x => x.Model));
            var constantOperand = new ConstantOperand("a");
            var containsOperator = new ContainsOperator(StringComparison.Ordinal);
            var binaryOperationModelContainsConst = new BinaryOperation(null, propertyOperandModel, constantOperand);

            // Act
            var containsExpression = containsOperator.Build(expressionBuilder, binaryOperationModelContainsConst);

            // Assert
            containsExpression.Should().NotBeNull();
            containsExpression.Type.Should().Be<bool>();

            var queryResult = TestHelper.ApplyExpression(CarPool.GetDefaultCarsList(), containsExpression, expressionBuilder.ParameterExpression);
            queryResult.Should().HaveCount(2);
            queryResult.Should().Contain(CarPool.carAudiA3); // Because of the 'a' in 'Sportback'
            queryResult.Should().Contain(CarPool.carAudiA4); // Because of the 'a' in 'Avant'
        }
    }
}