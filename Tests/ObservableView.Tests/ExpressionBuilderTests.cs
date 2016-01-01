using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using FluentAssertions;

using ObservableView.Tests.TestData;
using ObservableView;
using ObservableView.Filtering;
using ObservableView.Searching;
using ObservableView.Searching.Operators;

using Xunit;

namespace ObservableView.Tests
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
