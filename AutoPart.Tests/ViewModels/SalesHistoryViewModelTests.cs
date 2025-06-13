using Xunit;
using Microsoft.EntityFrameworkCore;
using AutoPartApp.ViewModels;
using AutoPart.DataAccess;
using AutoPart.Models;


namespace AutoPart.Tests.ViewModels
{
    public class SalesHistoryViewModelTests
    {
        [Fact]
        public void LoadSalesHistory_LoadsSalesFromDatabase()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AutoPartDbContext>()
                    .UseInMemoryDatabase("SalesHistoryTestDb")
                    .Options;
            using var context = new AutoPartDbContext(options);

            context.PartsInStock.Add(new Part { Id = "P1", Description = "Test Part" });
            context.PartSales.Add(new PartSale { PartId = "P1", Quantity = 5 });
            context.SaveChanges();

            var viewModel = new SalesHistoryViewModel(context);

            // Act
            viewModel.LoadSalesHistory();

            // Assert
            Assert.True(viewModel.SalesRows.Count > 0);
            Assert.Equal("P1", viewModel.SalesRows[0].PartId);
        }
    }
}