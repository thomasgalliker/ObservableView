using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

using FluentAssertions;

using ObservableView.Extensions;
using ObservableView.Tests.TestData;

using Xunit;

namespace ObservableView.Tests.Extensions
{
    public class QueryableExtensionsTests
    {
        [Fact]
        public void ShouldApplyContainsExpressionToWhereExtensionMethod()
        {
            // Arrange
            var parameterExpression = Expression.Parameter(typeof(Car), "x");
            Expression containsExpression = GetContainsExpression(parameterExpression, propertyName: "Model", propertyValue: "Golf");
            IEnumerable<Car> viewCollection = CarPool.GetDefaultCarsList();
            IQueryable<Car> queryableDtos = viewCollection.AsQueryable();

            // Act
            var whereList = queryableDtos.Where(containsExpression, parameterExpression).ToList();

            // Assert
            whereList.Should().NotBeNull();
            whereList.Should().HaveCount(1);
            whereList.Should().Contain(CarPool.carVwGolf);
        }

        private static Expression GetContainsExpression(ParameterExpression parameterExpression, string propertyName, string propertyValue)
        {
            var propertyExp = Expression.Property(parameterExpression, propertyName);
            MethodInfo containsMethodInfo = ReflectionHelper<string>.GetMethod<string>((source, argument) => source.Contains(argument));
            var someValue = Expression.Constant(propertyValue, typeof(string));
            var containsMethodExp = Expression.Call(propertyExp, containsMethodInfo, someValue);

            return containsMethodExp;
        }
    }
}