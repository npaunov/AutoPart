using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace AutoPartApp;

public class MainViewModel : INotifyPropertyChanged
{
    public ICommand ChangeLanguageCommand { get; }
    private string _selectedLanguage = "bg-BG";

    public MainViewModel()
    {
        // Set the initial culture to Bulgarian
        ChangeLanguageCommand = new RelayCommandGeneric<string>(ChangeLanguage);
        ChangeLanguageCommand.Execute(SelectedLanguage);
    }

    public string SelectedLanguage
    {
        get => _selectedLanguage;
        set
        {
            if (_selectedLanguage != value)
            {
                _selectedLanguage = value;
                OnPropertyChanged();
            }
        }
    }

    // Localized string properties
    public string AutoPartTitle => Properties.Strings.AutoPartTitle;
    public string WareHouseName => Properties.Strings.WareHouseName;
    public string AutoPartsName => Properties.Strings.AutoPartsName;
    public string LanguageName => Properties.Strings.LanguageName;
    public string EnglishName => Properties.Strings.EnglishName;
    public string BulgarianName => Properties.Strings.BulgarianName;

    private void ChangeLanguage(string? languageCode)
    {
        if (!string.IsNullOrEmpty(languageCode))
        {
            // Set the culture
            CultureInfo culture = new CultureInfo(languageCode);
            Thread.CurrentThread.CurrentUICulture = culture;
            Thread.CurrentThread.CurrentCulture = culture;

            // Update the resource culture
            Properties.Strings.Culture = culture;

            // Notify the UI to refresh bindings
            OnPropertyChanged(string.Empty); // Notify all properties
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}