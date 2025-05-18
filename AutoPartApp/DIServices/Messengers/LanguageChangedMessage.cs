using CommunityToolkit.Mvvm.Messaging.Messages;

namespace AutoPartApp;

/// <summary>
/// Message used to notify subscribers when the application's language or culture has changed.
/// Carries the new culture identifier as a string.
/// </summary>
public class LanguageChangedMessage : ValueChangedMessage<string>
{
    public LanguageChangedMessage(string newCulture) : base(newCulture) { }
}
