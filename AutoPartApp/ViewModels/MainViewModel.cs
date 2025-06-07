using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using AutoPartApp.Utilities;
using AutoPartApp.DIServices.Messengers;

namespace AutoPartApp.ViewModels;

public partial class MainViewModel : ObservableObject
{
    [ObservableProperty]
    private string _selectedLanguage = Properties.Strings.BulgarianCultureCode;

    public MainViewModel()
    {

    }

    [RelayCommand]
    private void ChangeLanguage(string newCulture)
    {
        LanguageUtil.ChangeLanguage(newCulture);
        WeakReferenceMessenger.Default.Send(new LanguageChangedMessage(newCulture));
    }
    // Localized string properties
    public string BulgarianCultureCode => Properties.Strings.BulgarianCultureCode;
    public string EnglishCultureCode => Properties.Strings.EnglishCultureCode;
    public string AutoPartTitle => Properties.Strings.AutoPartTitle;
    public string LanguageName => Properties.Strings.LanguageName;
    public string EnglishName => Properties.Strings.EnglishName;
    public string BulgarianName => Properties.Strings.BulgarianName;
    public string WareHouseName => Properties.Strings.WareHouseName;
    public string OrdersName => Properties.Strings.OrdersName;
    public string SaleHistoryName => Properties.Strings.SaleHistoryName;
    public string StoresName => Properties.Strings.StoresName;
    public string DataImportName => Properties.Strings.DataImportName;
}
