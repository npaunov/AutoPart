using Xunit;
using CommunityToolkit.Mvvm.Messaging;
using AutoPartApp.ViewModels;
using AutoPartApp.DIServices.Messengers;

namespace AutoPart.Tests.ViewModels
{
    public class MainViewModelTests
    {
        [Fact]
        /// <summary>
        /// Verifies that the ChangeLanguageCommand sends a LanguageChangedMessage when executed.
        /// </summary>
        public void ChangeLanguageCommand_SendsLanguageChangedMessage()
        {
            // Arrange
            bool messageReceived = false;
            WeakReferenceMessenger.Default
                .Register<LanguageChangedMessage>(this, (r, m) => messageReceived = true);

            var viewModel = new MainViewModel();

            // Act
            viewModel.ChangeLanguageCommand.Execute("en-US");

            // Assert
            Assert.True(messageReceived, $"{nameof(viewModel.ChangeLanguageCommand)}, did not send message");

            // Cleanup
            WeakReferenceMessenger.Default.UnregisterAll(this);
        }
    }
}

