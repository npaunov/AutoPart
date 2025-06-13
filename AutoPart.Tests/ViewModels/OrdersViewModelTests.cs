using Xunit;
using Moq;
using Microsoft.EntityFrameworkCore;
using AutoPartApp.ViewModels;
using AutoPartApp.DIServices.Services.Interfaces;
using AutoPart.DataAccess;

namespace AutoPart.Tests.ViewModels
{
    public class OrdersViewModelTests
    {
        [Fact]
        public void MonthsToOrder_SetAbove12_ShowsWarningAndClampsValue()
        {
            // Arrange
            var context = new AutoPartDbContext(new DbContextOptionsBuilder<AutoPartDbContext>()
                .UseInMemoryDatabase("OrdersTestDb").Options);
            var dialogServiceMock = new Mock<IDialogService>();
            var viewModel = new OrdersViewModel(context, dialogServiceMock.Object);

            // Act
            viewModel.MonthsToOrder = 15;

            // Assert
            Assert.Equal(12, viewModel.MonthsToOrder);
            dialogServiceMock
                .Verify(d => d.ShowMessage(It.Is<string>(msg => msg
                .Contains("cannot be greater than 12")),
                "Warning"), Times.Once , "Dialog message does not contain the correct text.");
        }
    }
}
