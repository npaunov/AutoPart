using AutoPartApp.Services;
using System.Globalization;
using System.Windows.Input;

namespace AutoPartApp;

public class MainViewModel : BaseViewModel
{
    private string _selectedLanguage = "bg-BG";
    public ICommand ChangeLanguageCommand { get; }

    public MainViewModel()
    {
        // Initialize the ChangeLanguageCommand
        ChangeLanguageCommand = new RelayCommandGeneric<string>(LanguageService.ChangeLanguage);
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
    public string BulgarianCultureCode => Properties.Strings.BulgarianCultureCode;
    public string EnglishCultureCode => Properties.Strings.EnglishCultureCode;
    public string AutoPartTitle => Properties.Strings.AutoPartTitle;
    public string WareHouseName => Properties.Strings.WareHouseName;
    public string AutoPartsName => Properties.Strings.AutoPartsName;
    public string LanguageName => Properties.Strings.LanguageName;
    public string EnglishName => Properties.Strings.EnglishName;
    public string BulgarianName => Properties.Strings.BulgarianName;
    public string DataImportName => Properties.Strings.DataImportName;
}
