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
            IExpressionBuilder expressionBuilder = new ExpressionBuilder(typeof(Car));

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

            var queryResult = TestHelper.ApplyExpression(CarPool.GetDefaultCarsList(), expression, expressionBuilder.ParameterExpression);
            queryResult.Should().HaveCount(1);
            queryResult.Should().Contain(CarPool.carVwGolf);
        }

        [Fact]
        public void ShouldTakeParameterExpressionInConstructor()
        {
            // Arrange
            var parameterExpression = Expression.Parameter(typeof(Car), "c");

            // Act
            var expressionBuilder = new ExpressionBuilder(parameterExpression: parameterExpression);

            // Assert
            expressionBuilder.ParameterExpression.Should().NotBeNull();
            expressionBuilder.ParameterExpression.Name.Should().Be("c");
        }

        [Fact]
        public void ShouldGenerateParameterExpressionBasedOnParameterType()
        {
            // Arrange
            var parameterType = typeof(Car);

            // Act
            var expressionBuilder = new ExpressionBuilder(parameterType: parameterType);

            // Assert
            expressionBuilder.ParameterExpression.Should().NotBeNull();
            expressionBuilder.ParameterExpression.Name.Should().Be("c");
        }
    }
}
