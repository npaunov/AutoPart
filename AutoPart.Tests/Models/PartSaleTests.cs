using Xunit;
using System;
using AutoPart.Models;

namespace AutoPart.Tests.Models
{
    /// <summary>
    /// Unit tests for the <see cref="PartSale"/> model.
    /// </summary>
    public class PartSaleTests
    {
        /// <summary>
        /// Verifies that a new <see cref="PartSale"/> instance has the expected default property values.
        /// </summary>
        [Fact]
        public void Constructor_ShouldInitializeWithDefaultValues()
        {
            // Arrange & Act
            var sale = new PartSale();

            // Assert
            Assert.Equal(0, sale.SaleId);
            Assert.Equal(string.Empty, sale.PartId);
            Assert.Equal(default(DateTime), sale.SaleDate);
            Assert.Equal(0, sale.Quantity);
            Assert.Null(sale.Part);
        }

        /// <summary>
        /// Verifies that a <see cref="PartSale"/> can reference a <see cref="Part"/> and that properties are set correctly.
        /// </summary>
        [Fact]
        public void CanReferencePart_ShouldSetPropertiesCorrectly()
        {
            // Arrange
            var part = new Part { Id = "P1" };
            var sale = new PartSale
            {
                PartId = part.Id,
                SaleDate = DateTime.Today,
                Quantity = 5,
                Part = part
            };

            // Assert
            Assert.Equal("P1", sale.PartId);
            Assert.Equal(part, sale.Part);
        }
    }
}