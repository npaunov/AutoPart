using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using System.Collections.ObjectModel;

namespace AutoPartApp;

/// <summary>
/// ViewModel for the Stores tab, managing store selection and order entry.
/// </summary>
public partial class StoresViewModel : ObservableObject
{
    /// <summary>List of available stores.</summary>
    [ObservableProperty]
    private ObservableCollection<string> _stores = new();

    [ObservableProperty]
    private string selectedStore;

    [ObservableProperty]
    private ObservableCollection<StoreOrderRowDto> orderRows = new();

    [ObservableProperty]
    private decimal totalBGN;

    [ObservableProperty]
    private decimal totalEuro;

    /// <summary>
    /// Initializes a new instance of the <see cref="StoresViewModel"/> class.
    /// </summary>
    public StoresViewModel()
    {
        OrderRows.CollectionChanged += (_, _) => RecalculateTotals();

        UpdateStores();
        // Listen for language change messages
        WeakReferenceMessenger.Default.Register<LanguageChangedMessage>(this, (r, m) =>
        {
            UpdateStores();
        });
    }

    partial void OnOrderRowsChanged(ObservableCollection<StoreOrderRowDto> value)
    {
        if (value != null)
        {
            foreach (var row in value)
                row.PropertyChanged += (_, __) => RecalculateTotals();
        }
        RecalculateTotals();
    }

    partial void OnSelectedStoreChanged(string value)
    {
        OrderRows.Clear();
        RecalculateTotals();
    }

    /// <summary>
    /// Adds an empty row to the order grid.
    /// </summary>
    public void AddEmptyRow()
    {
        var row = new StoreOrderRowDto();
        row.PropertyChanged += (_, __) => RecalculateTotals();
        OrderRows.Add(row);
    }

    /// <summary>
    /// Recalculates the total prices for all order rows.
    /// </summary>
    private void RecalculateTotals()
    {
        TotalBGN = OrderRows.Sum(r => r.TotalBGN);
        TotalEuro = OrderRows.Sum(r => r.TotalEuro);
    }

    private void UpdateStores()
    {
        // Clear and repopulate with localized store names
        Stores.Clear();
        Stores.Add(Properties.Strings.SofiaName);    // Use your localized resource
        Stores.Add(Properties.Strings.PlovdivName);  // Use your localized resource
    }

    #region Localization Properties
    public string StoresName => Properties.Strings.StoresName;
    public string SelectStoreName => Properties.Strings.SelectStoreName;
    public string PartIDName => Properties.Strings.PartIDName;
    public string DescriptionName => Properties.Strings.DescriptionName;
    public string QuantityName => Properties.Strings.QuantityName;
    public string TotalBGNName => Properties.Strings.TotalBGNName;
    public string TotalEuroName => Properties.Strings.TotalEuroName;
    public string TotalOrderPriceBGNName => Properties.Strings.TotalOrderPriceBGNName;
    public string TotalOrderPriceEUROName => Properties.Strings.TotalOrderPriceEUROName;
    #endregion

}
