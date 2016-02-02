using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

using FluentAssertions;

using ObservableView.Extensions;

using Xunit;

namespace ObservableView.Tests.Extensions
{
    public class QueryableExtensionsTests
    {
        [Fact]
        public void ShouldApplyWhereExpression()
        {
            // Arrange
            var parameterExpression = Expression.Parameter(typeof(Foo), "x");
            Expression containsExpression = GetExpression<Foo>(parameterExpression, "Bar", "a");
            var foo1 = new Foo { Bar = "aaa" };
            var foo2 = new Foo { Bar = "bbb" };
            IEnumerable<Foo> viewCollection = new List<Foo> { foo1, foo2 };
            IQueryable<Foo> queryableDtos = viewCollection.AsQueryable();

            // Act
            var whereList = queryableDtos.Where(containsExpression, parameterExpression).ToList();

            // Assert
            whereList.Should().NotBeNull();
            whereList.Should().HaveCount(1);
            whereList.Should().Contain(foo1);
        }

        class Foo
        {
            public string Bar { get; set; }
        }

        static Expression GetExpression<T>(ParameterExpression parameterExpression, string propertyName, string propertyValue)
        {
            var propertyExp = Expression.Property(parameterExpression, propertyName);
            MethodInfo containsMethodInfo = ReflectionHelper<string>.GetMethod<string>((source, argument) => source.Contains(argument));
            var someValue = Expression.Constant(propertyValue, typeof(string));
            var containsMethodExp = Expression.Call(propertyExp, containsMethodInfo, someValue);

            return containsMethodExp;
        }
    }
}