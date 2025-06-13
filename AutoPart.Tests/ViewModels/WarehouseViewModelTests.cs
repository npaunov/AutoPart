using Xunit;
using Microsoft.EntityFrameworkCore;
using AutoPartApp.ViewModels;
using AutoPart.Models;
using AutoPart.DataAccess;
using AutoPartApp.DIServices.Services;
using Moq;

namespace AutoPart.Tests.ViewModels
{
    public class WarehouseViewModelTests
    {
        [Fact]
        public void SearchCommand_FiltersPartsByIdOrDescription()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AutoPartDbContext>()
                .UseInMemoryDatabase(databaseName: "WarehouseTestDb1")
                .Options;
            using var context = new AutoPartDbContext(options);

            context.PartsInStock.AddRange(
                new Part { Id = "P1", Description = "Brake Pad" },
                new Part { Id = "P2", Description = "Oil Filter" }
            );
            context.SaveChanges();

            var currencyServiceMock = new Mock<CurrencySettingsService>();
            var viewModel = new WarehouseViewModel(context, currencyServiceMock.Object);

            // Act
            viewModel.SearchPartId = "Brake";
            viewModel.SearchCommand.Execute(null);

            // Assert
            Assert.Single(viewModel.Warehouse.PartsInStock);
            Assert.Equal("P1", viewModel.Warehouse.PartsInStock[0].Id);
        }

        [Fact]
        public void ClearCommand_ResetsSearchAndRestoresParts()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AutoPartDbContext>()
                .UseInMemoryDatabase(databaseName: "WarehouseTestDb2")
                .Options;
            using var context = new AutoPartDbContext(options);

            context.PartsInStock.AddRange(
                new Part { Id = "P1", Description = "Brake Pad" },
                new Part { Id = "P2", Description = "Oil Filter" }
            );
            context.SaveChanges();

            var currencyServiceMock = new Mock<CurrencySettingsService>();
            var viewModel = new WarehouseViewModel(context, currencyServiceMock.Object);

            // Simulate a search to filter the list
            viewModel.SearchPartId = "Brake";
            viewModel.SearchCommand.Execute(null);
            Assert.Single(viewModel.Warehouse.PartsInStock);

            // Act
            viewModel.ClearCommand.Execute(null);

            // Assert
            Assert.Equal(string.Empty, viewModel.SearchPartId);
            Assert.Equal(2, viewModel.Warehouse.PartsInStock.Count);
        }
    }
}