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
        public void ShouldUpperExpressionTest()
        {
            // Arrange
            const string InputString = "TeST";
            var parameter = Expression.Parameter(typeof(string), "x");

            // Act
            var toLowerExpression = parameter.ToUpper();
            var lambda = Expression.Lambda<Func<string, string>>(toLowerExpression, parameter);
            var func = lambda.Compile();
            var toLowerString = func(InputString);

            // Assert
            toLowerString.Should().Be("TEST");
        }

        [Fact]
        public void ShouldToStringExpressionTest()
        {
            // Arrange
            const int InputString = 1234;
            var parameter = Expression.Parameter(typeof(int), "x");

            // Act
            var toStringExpression = parameter.ToStringExpression();
            var lambda = Expression.Lambda<Func<int, string>>(toStringExpression, parameter);
            var func = lambda.Compile();
            var toString = func(InputString);

            // Assert
            toString.Should().Be(InputString.ToString());
        }
    }
}