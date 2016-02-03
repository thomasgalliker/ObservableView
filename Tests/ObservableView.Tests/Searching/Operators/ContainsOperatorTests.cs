using System.Linq.Expressions;

using FluentAssertions;

using ObservableView.Searching;
using ObservableView.Searching.Operators;
using ObservableView.Tests.TestData;

using Xunit;

namespace ObservableView.Tests.Searching.Operators
{
    public class ContainsOperatorTests
    {
        /// <summary>
        /// This test shall show how ExpressionBuilder behaves, when
        /// 1) a Contains operation which shall only be used for strings
        /// 2) is performed on an Int32 property
        /// 3) which does not contain a valid Int32 value (=null)
        /// </summary>
        [Fact]
        public void ShouldAllowContainsOperatorWithIntegerValues()
        {
            // Arrange
            ParameterExpression parameterExpression = Expression.Parameter(typeof(Car), "c");
            IExpressionBuilder expressionBuilder = new ExpressionBuilder(parameterExpression);

            ISearchSpecification<Car> searchSpecification = new SearchSpecification<Car>();
            searchSpecification.Add(car => car.Year, BinaryOperator.Contains);

            // Act
            var expression = expressionBuilder.Build(searchSpecification.BaseOperation);

            // Assert
            expression.Should().NotBeNull();
            expression.Type.Should().Be<bool>();
        }
    }
}
