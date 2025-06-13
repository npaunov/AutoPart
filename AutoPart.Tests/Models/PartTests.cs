using Xunit;
using AutoPart.Models;

namespace AutoPart.Tests.Models
{
    /// <summary>
    /// Unit tests for the <see cref="Part"/> model.
    /// </summary>
    public class PartTests
    {
        /// <summary>
        /// Verifies that a new <see cref="Part"/> instance has the expected default property values.
        /// </summary>
        [Fact]
        public void Constructor_ShouldInitializeWithDefaultValues()
        {
            // Arrange & Act
            var part = new Part();

            // Assert
            Assert.Equal(string.Empty, part.Id);
            Assert.Equal(string.Empty, part.Description);
            Assert.Equal(0, part.PriceBGN);
            Assert.Equal(0, part.Package);
            Assert.Equal(0, part.InStore);
            Assert.NotNull(part.Sales);
            Assert.Empty(part.Sales);
        }
    }
}