using System.Linq.Expressions;
using System.Reflection;

namespace ObservableView.Extensions
{
    public static class ExpressionExtensions
    {
        public static Expression ToLower(this Expression expression)
        {
            EnsureToString(ref expression);

            var methodInfo = ReflectionHelper<string>.GetMethod(source => source.ToLower());
            return Expression.Call(expression, methodInfo);
        }

        public static Expression ToUpper(this Expression expression)
        {
            EnsureToString(ref expression);

            var methodInfo = ReflectionHelper<string>.GetMethod(source => source.ToUpper());
            return Expression.Call(expression, methodInfo);
        }

        public static Expression Trim(this Expression expression)
        {
            EnsureToString(ref expression);

            var methodInfo = ReflectionHelper<string>.GetMethod(source => source.Trim());
            return Expression.Call(expression, methodInfo);
        }

        public static Expression ToStringExpression(this Expression expression)
        {
            var methodInfo = ReflectionHelper<string>.GetMethod(source => source.ToString());
            return Expression.Call(expression, methodInfo);
        }

        private static void EnsureToString(ref Expression expression)
        {
            if (expression.Type != typeof(string))
            {
                expression = expression.ToStringExpression();
            }
        }
    }
}