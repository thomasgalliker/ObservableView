using System.Linq.Expressions;

using FluentAssertions;

using ObservableView.Searching;
using ObservableView.Searching.Operators;
using ObservableView.Tests.TestData;

using Xunit;

namespace ObservableView.Tests.Searching
{
    public class ExpressionBuilderTests
    {
        [Fact]
        public void ShouldBuildBasicSearchSpecification()
        {
            // Arrange
            ParameterExpression parameterExpression = Expression.Parameter(typeof(Car), "c");
            IExpressionBuilder expressionBuilder = new ExpressionBuilder(parameterExpression);

            ISearchSpecification<Car> searchSpecification = new SearchSpecification<Car>();
            searchSpecification
                .Add(car => car.Model, BinaryOperator.Contains)
                .And(car => car.Year, BinaryOperator.Contains);

            // Act
            searchSpecification.ReplaceSearchTextVariables("20");
            var baseOperation = searchSpecification.BaseOperation;
            var expression = expressionBuilder.Build(baseOperation);

            // Assert
            expression.Should().NotBeNull();
            //TODO GATH: more asserts here
        }
    }
}
