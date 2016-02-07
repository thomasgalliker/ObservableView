﻿using System;
using System.Linq.Expressions;
using System.Reflection;

using FluentAssertions;

using ObservableView.Extensions;
using ObservableView.Searching;
using ObservableView.Searching.Operands;
using ObservableView.Searching.Operations;
using ObservableView.Searching.Operators;
using ObservableView.Tests.TestData;

using Xunit;

namespace ObservableView.Tests.Searching.Operators
{
    public class EqualOperatorTests
    {
        [Fact]
        public void ShouldCompareStringPropertyWithEqual()
        {
            // Arrange
            ParameterExpression parameterExpression = Expression.Parameter(typeof(Car), "c");
            IExpressionBuilder expressionBuilder = new ExpressionBuilder(parameterExpression);

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

            var queryResult = TestHelper.ApplyExpression(CarPool.GetDefaultCarsList(), expression, parameterExpression);
            queryResult.Should().HaveCount(1);
            queryResult.Should().Contain(CarPool.carAudiA1);
        }

        [Fact]
        public void ShouldCompareIntegerPropertyWithEqual()
        {
            // Arrange
            ParameterExpression parameterExpression = Expression.Parameter(typeof(Car), "c");
            IExpressionBuilder expressionBuilder = new ExpressionBuilder(parameterExpression);

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

            var queryResult = TestHelper.ApplyExpression(CarPool.GetDefaultCarsList(), expression, parameterExpression);
            queryResult.Should().HaveCount(1);
            queryResult.Should().Contain(CarPool.carBmwM3);
        }

        [Fact]
        public void ShouldCompareReferenceTypePropertyWithEqual()
        {
            // Arrange
            ParameterExpression parameterExpression = Expression.Parameter(typeof(Car), "c");
            IExpressionBuilder expressionBuilder = new ExpressionBuilder(parameterExpression);

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

            var queryResult = TestHelper.ApplyExpression(CarPool.GetDefaultCarsList(), expression, parameterExpression);
            queryResult.Should().HaveCount(1);
            queryResult.Should().Contain(CarPool.carVwGolf);
        }

        [Fact]
        public void ShouldCompareNullReferenceTypePropertyWithEqual()
        {
            // Arrange
            ParameterExpression parameterExpression = Expression.Parameter(typeof(Car), "c");
            IExpressionBuilder expressionBuilder = new ExpressionBuilder(parameterExpression);

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

            var queryResult = TestHelper.ApplyExpression(CarPool.GetDefaultCarsList(), expression, parameterExpression);
            queryResult.Should().HaveCount(1);
            queryResult.Should().Contain(CarPool.carAudiA4);
        }

        [Fact]
        public void ShouldThrowInvalidOperationExceptionIfSourceAndTargetTypeMismatch()
        {
            // Arrange
            ParameterExpression parameterExpression = Expression.Parameter(typeof(Car), "c");
            IExpressionBuilder expressionBuilder = new ExpressionBuilder(parameterExpression);

            var propertyInfo = ReflectionHelper<Car>.GetProperty(c => c.Year);
            PropertyOperand propertyOperand = new PropertyOperand(propertyInfo, expressionProcessors: null);
            const string ConstantYearValue = "2012";
            IOperand constantOperand = new ConstantOperand(value: ConstantYearValue);

            // Act
            BinaryOperator equalOperator = new EqualOperator();
            var binaryOperation = new BinaryOperation(equalOperator, propertyOperand, constantOperand);
            Action action = () => equalOperator.Build(expressionBuilder, binaryOperation);

            // Assert
            action.ShouldThrow<InvalidOperationException>();
        }

        [Fact]
        public void ShouldNotThrowIfConstantOperandIsConvertedToPropertyType()
        {
            // Arrange
            ParameterExpression parameterExpression = Expression.Parameter(typeof(Car), "c");
            IExpressionBuilder expressionBuilder = new ExpressionBuilder(parameterExpression);

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

            var queryResult = TestHelper.ApplyExpression(CarPool.GetDefaultCarsList(), expression, parameterExpression);
            queryResult.Should().HaveCount(1);
            queryResult.Should().Contain(CarPool.carBmwM3);
        }
    }
}
