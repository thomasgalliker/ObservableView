using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace ObservableView.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> Where<T>(this IQueryable<T> source, Expression baseExpression, ParameterExpression parameterExpression)
        {
            // TODO: Use ReflectionHelper here
            //MethodInfo whereMethodInfo = ReflectionHelper<IEnumerable<object>>.GetMethod<Func<object, bool>>((x, arg) => x.Where(arg));

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