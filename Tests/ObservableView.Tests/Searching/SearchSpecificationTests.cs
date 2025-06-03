using ObservableView.Searching.Operands;
using ObservableView.Searching.Processors;

namespace ObservableView.Tests.Searching
{
    public class SearchSpecificationTests
    {
        [Fact]
        public void ShouldAddSearchSpecification()
        {
            // Arrange
            ISearchSpecification<Car> searchSpecification = new SearchSpecification<Car>();
            searchSpecification
                .Add(car => car.Model, BinaryOperator.Contains)
                .Or(car => car.Year, BinaryOperator.Contains)
                .And(car => car.Brand, BinaryOperator.Contains);

            // Act
            var sequenceOfExpressions = searchSpecification.BaseOperation.Flatten().ToList();

            // Assert
            sequenceOfExpressions.Should().HaveCount(11);

            sequenceOfExpressions.ElementAt(0).Should().BeOfType(typeof(PropertyOperand));
            sequenceOfExpressions.ElementAt(1).Should().BeOfType(typeof(ContainsOperator));
            sequenceOfExpressions.ElementAt(2).Should().BeOfType(typeof(VariableOperand));

            sequenceOfExpressions.ElementAt(3).Should().BeOfType(typeof(OrOperator));

            sequenceOfExpressions.ElementAt(4).Should().BeOfType(typeof(PropertyOperand));
            sequenceOfExpressions.ElementAt(5).Should().BeOfType(typeof(ContainsOperator));
            sequenceOfExpressions.ElementAt(6).Should().BeOfType(typeof(VariableOperand));

            sequenceOfExpressions.ElementAt(7).Should().BeOfType(typeof(AndOperator));

            sequenceOfExpressions.ElementAt(8).Should().BeOfType(typeof(PropertyOperand));
            sequenceOfExpressions.ElementAt(9).Should().BeOfType(typeof(ContainsOperator));
            sequenceOfExpressions.ElementAt(10).Should().BeOfType(typeof(VariableOperand));
        }

        [Fact]
        public void ShouldAddOrOperatorForSubsequentAdds()
        {
            // Arrange
            ISearchSpecification<Car> searchSpecification = new SearchSpecification<Car>();
            searchSpecification
                .Add(car => car.Model, BinaryOperator.Contains)
                .Add(car => car.Year, BinaryOperator.Contains);

            // Act
            var sequenceOfExpressions = searchSpecification.BaseOperation.Flatten().ToList();

            // Assert
            sequenceOfExpressions.Should().HaveCount(7);

            sequenceOfExpressions.ElementAt(0).Should().BeOfType(typeof(PropertyOperand));
            sequenceOfExpressions.ElementAt(1).Should().BeOfType(typeof(ContainsOperator));
            sequenceOfExpressions.ElementAt(2).Should().BeOfType(typeof(VariableOperand));

            sequenceOfExpressions.ElementAt(3).Should().BeOfType(typeof(OrOperator));

            sequenceOfExpressions.ElementAt(4).Should().BeOfType(typeof(PropertyOperand));
            sequenceOfExpressions.ElementAt(5).Should().BeOfType(typeof(ContainsOperator));
            sequenceOfExpressions.ElementAt(6).Should().BeOfType(typeof(VariableOperand));
        }

        [Fact]
        public void ShouldReplaceSearchTextVariables()
        {
            // Arrange
            IExpressionBuilder expressionBuilder = new ExpressionBuilder(typeof(Car));

            const string SearchText = "test";
            ISearchSpecification<Car> searchSpecification = new SearchSpecification<Car>();
            searchSpecification
               .Add(car => car.Model, BinaryOperator.Contains)
               .And(car => car.Year, BinaryOperator.Contains);

            // Act
            searchSpecification.ReplaceSearchTextVariables(SearchText);
            var expression = expressionBuilder.Build(searchSpecification.BaseOperation);

            // Assert
            var sequenceOfExpressions = searchSpecification.BaseOperation.Flatten().ToList();
            sequenceOfExpressions.Should().HaveCount(7);

            var variableOperand1 = sequenceOfExpressions.ElementAt(2) as VariableOperand;
            variableOperand1.Value.Should().Be(SearchText);

            var variableOperand2 = sequenceOfExpressions.ElementAt(6) as VariableOperand;
            variableOperand2.Value.Should().Be(SearchText);

            var queryResult = TestHelper.ApplyExpression(CarPool.GetDefaultCarsList(), expression, expressionBuilder.ParameterExpression);
            queryResult.Should().HaveCount(0);
        }

        [Fact]
        public void ShouldReplaceSearchTextVariablesWithNull()
        {
            // Arrange
            IExpressionBuilder expressionBuilder = new ExpressionBuilder(typeof(Car));

            const object SearchText = null;
            ISearchSpecification<Car> searchSpecification = new SearchSpecification<Car>();
            searchSpecification
               .Add(car => car.Model, BinaryOperator.Contains)
               .Or(car => car.ChasisNumber, BinaryOperator.Equal)
               .Or(car => car.Year, BinaryOperator.Contains);

            // Act
            searchSpecification.ReplaceSearchTextVariables(SearchText);
            var expression = expressionBuilder.Build(searchSpecification.BaseOperation);

            // Assert
            var sequenceOfExpressions = searchSpecification.BaseOperation.Flatten().ToList();
            sequenceOfExpressions.Should().HaveCount(11);

            var variableOperand1 = sequenceOfExpressions.ElementAt(2) as VariableOperand;
            variableOperand1.Value.Should().Be(SearchText);

            var variableOperand2 = sequenceOfExpressions.ElementAt(6) as VariableOperand;
            variableOperand2.Value.Should().Be(SearchText);

            var queryResult = TestHelper.ApplyExpression(CarPool.GetDefaultCarsList(), expression, expressionBuilder.ParameterExpression);
            queryResult.Should().HaveCount(1);
            queryResult.Should().Contain(CarPool.carAudiA4); // Audi A4 has ChasisNumber == null
        }

        [Fact]
        public void ShouldSequentiallyApplyExpressionProcessors()
        {
            // Arrange
            IExpressionBuilder expressionBuilder = new ExpressionBuilder(typeof(Car));

            ISearchSpecification<Car> searchSpecification = new SearchSpecification<Car>();
            searchSpecification.Add(c => c.ChasisNumber, new IExpressionProcessor[] { ExpressionProcessor.ToLower, ExpressionProcessor.Trim }, new ContainsOperator(StringComparison.Ordinal));

            // Act
            searchSpecification.ReplaceSearchTextVariables("aa");
            var expression = expressionBuilder.Build(searchSpecification.BaseOperation);

            // Assert
            var sequenceOfExpressions = searchSpecification.BaseOperation.Flatten().ToList();
            sequenceOfExpressions.Should().HaveCount(3);
            sequenceOfExpressions.ElementAt(0).Should().BeOfType(typeof(PropertyOperand));
            sequenceOfExpressions.ElementAt(1).Should().BeOfType(typeof(ContainsOperator));
            sequenceOfExpressions.ElementAt(2).Should().BeOfType(typeof(VariableOperand));

            sequenceOfExpressions.ElementAt(0).As<PropertyOperand>().ExpressionProcessors.Should().HaveCount(2);
            sequenceOfExpressions.ElementAt(0).As<PropertyOperand>().ExpressionProcessors.ElementAt(0).Should().BeOfType(typeof(ToLowerExpressionProcessor));
            sequenceOfExpressions.ElementAt(0).As<PropertyOperand>().ExpressionProcessors.ElementAt(1).Should().BeOfType(typeof(TrimExpressionProcessor));

            var queryResult = TestHelper.ApplyExpression(CarPool.GetDefaultCarsList(), expression, expressionBuilder.ParameterExpression);
            queryResult.Should().HaveCount(2);
            queryResult.Should().Contain(CarPool.carAudiA1);
            queryResult.Should().Contain(CarPool.carAudiA3);
        }
    }
}
