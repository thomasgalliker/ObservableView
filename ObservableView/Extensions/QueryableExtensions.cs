using System;
using System.Linq;
using System.Linq.Expressions;

namespace ObservableView.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> Where<T>(this IQueryable<T> source, Expression baseExpression, ParameterExpression parameterExpression)
        {
            MethodCallExpression whereCallExpression = Expression.Call(
                typeof(Queryable),
                "Where",
                new[] { source.ElementType },
                source.Expression,
                Expression.Lambda<Func<T, bool>>(baseExpression, new[] { parameterExpression }));

            return source.Provider.CreateQuery<T>(whereCallExpression);
        }
    }
}