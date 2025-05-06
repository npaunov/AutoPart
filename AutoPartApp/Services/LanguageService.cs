using System;

namespace AutoPartApp.Services;

/// <summary>
/// A service that facilitates language change across view models.
/// </summary>
public class LanguageService
{
    /// <summary>
    /// Event triggered when the application language is changed.
    /// </summary>
    public event Action<string>? LanguageChanged;

    /// <summary>
    /// Notifies subscribers about the language change.
    /// </summary>
    /// <param name="newCulture">The new culture code (e.g., "en-EN", "bg-BG").</param>
    public void ChangeLanguage(string newCulture)
    {
        LanguageChanged?.Invoke(newCulture);
    }
}
