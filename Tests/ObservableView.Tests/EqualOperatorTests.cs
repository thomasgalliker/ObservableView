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
    public class EqualOperatorTests
    {
        [Fact]
        public void ShouldComparePropertyWithEqual()
        {
            // Arrange
            ParameterExpression parameterExpression = Expression.Parameter(typeof(Car), "c");
            IExpressionBuilder expressionBuilder = new ExpressionBuilder(parameterExpression);

            ISearchSpecification<Car> searchSpecification = new SearchSpecification<Car>();
            searchSpecification.Add(car => car.Year, BinaryOperator.Equals);

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
            searchSpecification.Add(car => car.Year, BinaryOperator.Equals);

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
