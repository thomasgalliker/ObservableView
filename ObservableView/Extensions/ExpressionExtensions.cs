using System;
using System.Linq.Expressions;
using System.Reflection;

namespace ObservableView.Extensions
{
    internal static class ExpressionExtensions
    {
        internal static Expression ToLower(this Expression expression)
        {
            var methodInfo = typeof(string).GetRuntimeMethod("ToLower", new Type[] { });
            return Expression.Call(expression, methodInfo);
        }

        internal static Expression Contains(this Expression expression, Expression containsExpression)
        {
            var methodInfo = typeof(string).GetRuntimeMethod("Contains", new[] { typeof(string) });
            return Expression.Call(expression, methodInfo, containsExpression);
        }
    }
}