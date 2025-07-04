using System.Globalization;

namespace AutoPart.Utilities;

/// <summary>
/// A service that facilitates language change across view models.
/// </summary>
public static class LanguageUtil
{
    /// <summary>
    /// Changes the application language and notifies subscribers.
    /// </summary>
    /// <param name="newCulture">The new culture code (e.g., "en-EN", "bg-BG").</param>
    public static void ChangeLanguage(string newCulture)
    {
        if (!string.IsNullOrEmpty(newCulture))
        {
            // Set the culture
            CultureInfo culture = new CultureInfo(newCulture);
            Thread.CurrentThread.CurrentUICulture = culture;
            Thread.CurrentThread.CurrentCulture = culture;
        }
    }
}
