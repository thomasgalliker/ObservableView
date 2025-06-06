﻿using ObservableView.Searching.Operands;
using ObservableView.Searching.Processors;

namespace ObservableView.Tests.Searching.Operands
{
    public class PropertyOperandTests
    {
        [Fact]
        public void ShouldBuildWithoutExpressionProcessors()
        {
            // Arrange
            IExpressionBuilder expressionBuilder = new ExpressionBuilder(typeof(Car));

            var propertyInfo = ReflectionHelper<Car>.GetProperty(x => x.Model);
            var propertyOperand = new PropertyOperand(propertyInfo: propertyInfo, expressionProcessors: null);

            // Act
            var propertyExpression = propertyOperand.Build(expressionBuilder);

            // Assert
            propertyExpression.Should().NotBeNull();
            propertyExpression.Type.Should().Be<string>();

            var lambda = Expression.Lambda<Func<Car, string>>(propertyExpression, expressionBuilder.ParameterExpression);
            var func = lambda.Compile();
            var modelName = func(CarPool.carVwGolf);

            modelName.Should().Be(CarPool.carVwGolf.Model);
        }

        [Fact]
        public void ShouldBuildWithExpressionProcessors()
        {
            // Arrange
            IExpressionBuilder expressionBuilder = new ExpressionBuilder(typeof(Car));

            var propertyInfo = ReflectionHelper<Car>.GetProperty(x => x.Model);
            var expressionProcessors = new IExpressionProcessor[] { ExpressionProcessor.ToLower, ExpressionProcessor.ToUpper };
            var propertyOperand = new PropertyOperand(propertyInfo: propertyInfo, expressionProcessors: expressionProcessors);

            // Act
            var propertyExpression = propertyOperand.Build(expressionBuilder);

            // Assert
            propertyExpression.Should().NotBeNull();
            propertyExpression.Type.Should().Be<string>();

            var lambda = Expression.Lambda<Func<Car, string>>(propertyExpression, expressionBuilder.ParameterExpression);
            var func = lambda.Compile();
            var modelName = func(CarPool.carVwGolf);

            modelName.Should().Be(CarPool.carVwGolf.Model.ToUpper());
        }
    }
}