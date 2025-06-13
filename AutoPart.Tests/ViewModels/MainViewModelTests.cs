using Xunit;
using CommunityToolkit.Mvvm.Messaging;
using AutoPartApp.ViewModels;
using AutoPartApp.DIServices.Messengers;

namespace AutoPart.Tests.ViewModels
{
    public class MainViewModelTests
    {
        [Fact]
        public void ChangeLanguageCommand_SendsLanguageChangedMessage()
        {
            // Arrange
            bool messageReceived = false;
            WeakReferenceMessenger.Default.Register<LanguageChangedMessage>(this, (r, m) => messageReceived = true);

            var viewModel = new MainViewModel();

            // Act
            viewModel.ChangeLanguageCommand.Execute("en-US");

            // Assert
            Assert.True(messageReceived);

            // Cleanup
            WeakReferenceMessenger.Default.UnregisterAll(this);
        }
    }
}

