namespace ObservableView.Tests.TestData
{
    internal static class TestHelper
    {
        internal static IList<Car> ApplyExpression(IEnumerable<Car> cars, Expression expression, ParameterExpression parameterExpression)
        {
            IQueryable<Car> queryableDtos = cars.AsQueryable();
            return queryableDtos.Where(expression, parameterExpression).ToList();
        }
    }
}