using System;
using System.Globalization;
using System.Threading;
using System.Windows;

namespace AutoPartApp.Services;

/// <summary>
/// A service that facilitates language change across view models.
/// </summary>
public static class LanguageService
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

            // Refresh the UI
            if (Application.Current.MainWindow != null)
            {
                Application.Current.MainWindow.Language = System.Windows.Markup.XmlLanguage.GetLanguage(culture.IetfLanguageTag);
            }
        }
    }
}
