namespace Services.DIServices.Interfaces;

/// <summary>
/// Provides an abstraction for displaying dialog messages to the user.
/// </summary>
public interface IDialogService
{
    /// <summary>
    /// Shows a confirmation dialog with the specified message and title.
    /// </summary>
    /// <param name="message">The message to display in the dialog.</param>
    /// <param name="title">The title of the dialog window.</param>
    /// <returns>True if the user clicks Yes; otherwise, false.</returns>
    bool ShowConfirmation(string message, string title);
}
