using System;
using System.Collections.Generic;
using System.Linq;

using FluentAssertions;

using ObservableView.Sorting;

using Xunit;

namespace ObservableView.Tests.Sorting
{
    public class NaturalSortComparableTests
    {
        private static List<NaturalSortComparable> GetTestData()
        {
            var defaultInputList = new List<NaturalSortComparable>
            {
                new NaturalSortComparable("10"),
                new NaturalSortComparable("A100b"),
                new NaturalSortComparable("A99b20"),
                new NaturalSortComparable("A99b4"),
                new NaturalSortComparable("Z88"),
                new NaturalSortComparable("1"),
                new NaturalSortComparable("99"),
                new NaturalSortComparable("0"),
            };

            // Make sure, the tests don't depend on a certain preordering.
            Shuffle(defaultInputList);

            return defaultInputList;
        }

        [Fact]
        public void ShouldNaturalSort()
        {
            // Arrange
            const string FirstString = "0";
            const string LastString = "Z88";
            var inputList = GetTestData();

            // Act
            inputList.Sort();

            // Assert
            inputList.First().ToString().Should().Be(FirstString);
            inputList.Last().ToString().Should().Be(LastString);
        }

        [Fact]
        public void ShouldNaturalSortNumericValues()
        {
            // Arrange
            const string FirstString = "A99b20";
            const string LastString = "A100b";

            var testList = new List<NaturalSortComparable>
            {
                new NaturalSortComparable("A100b"),
                new NaturalSortComparable("A99b20"),
            };
            Shuffle(testList);

            // Act
            testList.Sort();

            // Assert
            testList.First().ToString().Should().Be(FirstString);
            testList.Last().ToString().Should().Be(LastString);
        }

        [Fact]
        public void ShouldNaturalSortEmptyStrings()
        {
            // Arrange
            const string FirstString = "";
            const string LastString = "Z88";
            var inputList = GetTestData();
            inputList.Add(new NaturalSortComparable(string.Empty));
            Shuffle(inputList);

            // Act
            inputList.Sort();

            // Assert
            inputList.First().ToString().Should().Be(FirstString);
            inputList.Last().ToString().Should().Be(LastString);
        }

        [Fact]
        public void ShouldNaturalSortNullValues()
        {
            // Arrange
            const string FirstString = "";
            const string LastString = "Z88";
            var inputList = GetTestData();
            inputList.Add(new NaturalSortComparable(null));
            Shuffle(inputList);

            // Act
            inputList.Sort();

            // Assert
            inputList.First().ToString().Should().Be(FirstString);
            inputList.Last().ToString().Should().Be(LastString);
        }

        private static void Shuffle<T>(IList<T> list)
        {
            var rng = new Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}