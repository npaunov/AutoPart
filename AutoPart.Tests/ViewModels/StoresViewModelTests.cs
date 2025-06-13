using Xunit;
using Microsoft.EntityFrameworkCore;
using AutoPartApp.ViewModels;
using AutoPart.DataAccess;

namespace AutoPart.Tests.ViewModels
{
    public class StoresViewModelTests
    {
        [Fact]
        public void AddEmptyRowCommand_AddsRow_WhenLastRowIsValid()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AutoPartDbContext>()
                .UseInMemoryDatabase("StoresTestDb")
                .Options;
            using var context = new AutoPartDbContext(options);

            var viewModel = new StoresViewModel(context);

            // Act
            viewModel.AddEmptyRowCommand.Execute(null);

            // Assert
            Assert.True(viewModel.OrderRows.Count > 0);
        }
    }
}

