namespace ObservableView.Tests.Extensions
{
    public class StringExtensionsTests
    {
        [Fact]
        public void ShouldReturnTrueIfFullMatch()
        {
            // Arrange
            const string BaseString = "abcdefg";

            // Act
            var isContained = BaseString.Contains(BaseString);

            // Assert
            isContained.Should().BeTrue();
        }

        [Fact]
        public void ShouldReturnTrueIfPartialMatch()
        {
            // Arrange
            const string BaseString = "abcdefg";

            // Act
            var isContained = BaseString.Contains("cd");

            // Assert
            isContained.Should().BeTrue();
        }

        [Fact]
        public void ShouldReturnTrueIfOrdinalComparisonSucceeds()
        {
            // Arrange
            const string BaseString = "abcdefg";

            // Act
            var isContained = BaseString.Contains("cd", StringComparison.Ordinal);

            // Assert
            isContained.Should().BeTrue();
        }

        [Fact]
        public void ShouldReturnFalseIfOrdinalComparisonFails()
        {
            // Arrange
            const string BaseString = "abcdefg";

            // Act
            var isContained = BaseString.Contains("cD", StringComparison.Ordinal);

            // Assert
            isContained.Should().BeFalse();
        }
    }
}