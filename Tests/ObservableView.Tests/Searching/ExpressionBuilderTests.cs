using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using FluentAssertions;

using ObservableView.Extensions;
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
                .And(car => car.ChasisNumber, BinaryOperator.Contains);

            // Act
            searchSpecification.ReplaceSearchTextVariables("GOLF");
            var expression = expressionBuilder.Build(searchSpecification.BaseOperation);

            // Assert
            expression.Should().NotBeNull();
            expression.Type.Should().Be<bool>();

            var queryResult = TestHelper.ApplyExpression(CarPool.GetDefaultCarsList(), expression, parameterExpression);
            queryResult.Should().Contain(CarPool.carVwGolf);
        }
    }
}
