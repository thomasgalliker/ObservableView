using System.Linq.Expressions;

using FluentAssertions;

using ObservableView.Searching;
using ObservableView.Searching.Operators;
using ObservableView.Tests.TestData;

using Xunit;

namespace ObservableView.Tests.Searching
{
    public class EqualOperatorTests
    {
        [Fact]
        public void ShouldComparePropertyWithEqual()
        {
            // Arrange
            ParameterExpression parameterExpression = Expression.Parameter(typeof(Car), "c");
            IExpressionBuilder expressionBuilder = new ExpressionBuilder(parameterExpression);

            ISearchSpecification<Car> searchSpecification = new SearchSpecification<Car>();
            searchSpecification.Add(car => car.Year, BinaryOperator.Equal);

            // Act
            searchSpecification.ReplaceSearchTextVariables("2000");
            var baseOperation = searchSpecification.BaseOperation;
            var expression = expressionBuilder.Build(baseOperation);

            // Assert
            expression.Should().NotBeNull();
            //TODO GATH: more asserts here
        }

        [Fact]
        public void ShouldComparePropertyWithEqualNullValue()
        {
            // Arrange
            ParameterExpression parameterExpression = Expression.Parameter(typeof(Car), "c");
            IExpressionBuilder expressionBuilder = new ExpressionBuilder(parameterExpression);

            ISearchSpecification<Car> searchSpecification = new SearchSpecification<Car>();
            searchSpecification.Add(car => car.Year, BinaryOperator.Equal);

            // Act
            searchSpecification.ReplaceSearchTextVariables((object)null);
            var baseOperation = searchSpecification.BaseOperation;
            var expression = expressionBuilder.Build(baseOperation);

            // Assert
            expression.Should().NotBeNull();
            //TODO GATH: more asserts here
        }
    }
}
