using System.Windows;

namespace AutoPartApp;

/// <summary>
/// Provides a concrete implementation of <see cref="IDialogService"/> for displaying dialog messages using WPF's MessageBox.
/// </summary>
public class DialogService : IDialogService
{
    /// <summary>
    /// Shows a confirmation dialog with the specified message and title.
    /// </summary>
    /// <param name="message">The message to display in the dialog.</param>
    /// <param name="title">The title of the dialog window.</param>
    /// <returns>True if the user clicks Yes; otherwise, false.</returns>
    public bool ShowConfirmation(string message, string title)
    {
        var result = MessageBox.Show(
            message,
            title,
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);

        return result == MessageBoxResult.Yes;
    }
}
