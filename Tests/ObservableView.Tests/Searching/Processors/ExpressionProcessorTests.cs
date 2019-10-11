using System;
using System.Linq.Expressions;

using FluentAssertions;

using ObservableView.Searching.Processors;

using Xunit;

namespace ObservableView.Tests.Searching.Processors
{
    public class ExpressionProcessorTests
    {
        [Fact]
        public void ShouldProcessStringToLower()
        {
            // Arrange
            const string InputString = "AbCD";
            const string ExpectedOutput = "abcd";
            var expression = Expression.Constant(InputString);

            // Act
            IExpressionProcessor processor = new ToLowerExpressionProcessor();
            var processedExpression = processor.Process(expression);

            // Assert
            expression.Should().NotBeNull();
            expression.Type.Should().Be<string>();

            var lambdaExpr = Expression.Lambda<Func<string>>(processedExpression);
            var func = lambdaExpr.Compile();
            var result = func();
            result.Should().Be(ExpectedOutput);
        }

        [Fact]
        public void ShouldProcessStringTrim()
        {
            // Arrange
            const string InputString = " text with spaces  ";
            const string ExpectedOutput = "text with spaces";
            var expression = Expression.Constant(InputString);

            // Act
            IExpressionProcessor processor = new TrimExpressionProcessor();
            var processedExpression = processor.Process(expression);

            // Assert
            expression.Should().NotBeNull();
            expression.Type.Should().Be<string>();

            var lambdaExpr = Expression.Lambda<Func<string>>(processedExpression);
            var func = lambdaExpr.Compile();
            var result = func();
            result.Should().Be(ExpectedOutput);
        }
    }
}