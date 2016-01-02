using System;
using System.Linq.Expressions;

using ObservableView.Extensions;
using FluentAssertions;
using Xunit;

namespace ObservableView.Tests.Extensions
{
    public class ExpressionExtensionsTests
    {
        [Fact]
        public void ShouldLowerExpressionTest()
        {
            // Arrange
            const string InputString = "TeST";
            var parameter = Expression.Parameter(typeof(string), "x");

            // Act
            var toLowerExpression = parameter.ToLower();
            var lambda = Expression.Lambda<Func<string, string>>(toLowerExpression, parameter);
            var func = lambda.Compile();
            var toLowerString = func(InputString);

            // Assert
            toLowerString.Should().Be("test");
        }

        [Fact]
        public void ShouldContainExpressionTest()
        {
            // Arrange
            const string InputString = "testtesttesttest";
            var parameter = Expression.Parameter(typeof(string), "x");
            var testConstant = Expression.Constant("test");

            // Act
            var containsExpression = parameter.Contains(testConstant);
            var lambda = Expression.Lambda<Func<string, bool>>(containsExpression, parameter);
            var func = lambda.Compile();
            var isContained = func(InputString);

            // Assert
            isContained.Should().BeTrue();
        }

        [Fact]
        public void ShouldNotContainExpressionTest()
        {
            // Arrange
            const string InputString = "nonononono";
            var parameter = Expression.Parameter(typeof(string), "x");
            var testConstant = Expression.Constant("test");

            // Act
            var containsExpression = parameter.Contains(testConstant);
            var lambda = Expression.Lambda<Func<string, bool>>(containsExpression, parameter);
            var func = lambda.Compile();
            var isContained = func(InputString);

            // Assert
            isContained.Should().BeFalse();
        }
    }
}