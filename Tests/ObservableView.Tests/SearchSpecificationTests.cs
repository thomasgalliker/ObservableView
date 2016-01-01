using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using FluentAssertions;

using ObservableView.Tests.TestData;
using ObservableView;
using ObservableView.Extensions;
using ObservableView.Filtering;
using ObservableView.Searching;
using ObservableView.Searching.Operands;
using ObservableView.Searching.Operations;
using ObservableView.Searching.Operators;

using Xunit;

namespace ObservableView.Tests
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
            const string SearchText = "test";
            ISearchSpecification<Car> searchSpecification = new SearchSpecification<Car>();
            searchSpecification
               .Add(car => car.Model, BinaryOperator.Contains)
               .And(car => car.Year, BinaryOperator.Contains);

            // Act
            searchSpecification.ReplaceSearchTextVariables(SearchText);

            // Assert
            var sequenceOfExpressions = searchSpecification.BaseOperation.Flatten().ToList();
            sequenceOfExpressions.Should().HaveCount(7);

            var variableOperand1 = sequenceOfExpressions.ElementAt(2) as VariableOperand;
            variableOperand1.Value.Should().Be(SearchText);

            var variableOperand2 = sequenceOfExpressions.ElementAt(6) as VariableOperand;
            variableOperand2.Value.Should().Be(SearchText);
        }
    }
}
