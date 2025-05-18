using CommunityToolkit.Mvvm.Messaging.Messages;

namespace AutoPartApp;

public class LanguageChangedMessage : ValueChangedMessage<string>
{
    public LanguageChangedMessage(string newCulture) : base(newCulture) { }
}
