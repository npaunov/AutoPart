using CommunityToolkit.Mvvm.Messaging.Messages;

namespace Services;

public class LanguageChangedMessage : ValueChangedMessage<string>
{
    public LanguageChangedMessage(string newCulture) : base(newCulture) { }
}
