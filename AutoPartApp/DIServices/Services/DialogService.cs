using System.Windows;

namespace AutoPartApp;

/// <summary>
/// Provides a concrete implementation of <see cref="IDialogService"/> for displaying dialog messages using WPF's MessageBox.
/// </summary>
public class DialogService : IDialogService
{
    public bool ShowConfirmation(string message, string title)
    {
        var result = MessageBox.Show(
            message,
            title,
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);

        return result == MessageBoxResult.Yes;
    }

    /// <summary>
    /// Shows an informational or warning dialog with an OK button.
    /// </summary>
    /// <param name="message">The message to display in the dialog.</param>
    /// <param name="title">The title of the dialog window.</param>
    public void ShowMessage(string message, string title)
    {
        MessageBox.Show(
            message,
            title,
            MessageBoxButton.OK,
            MessageBoxImage.Warning);
    }
}