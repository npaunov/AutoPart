using AutoPartApp.EntityFramework;
using AutoPartApp.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace AutoPartApp;

/// <summary>
/// ViewModel for the Stores tab, managing store selection, order entry, and part suggestions/validation.
/// </summary>
public partial class StoresViewModel : ObservableObject
{
    /// <summary>
    /// List of available stores (localized).
    /// </summary>
    [ObservableProperty]
    private ObservableCollection<string> _stores = new();

    /// <summary>
    /// The currently selected store.
    /// </summary>
    [ObservableProperty]
    private string selectedStore;

    /// <summary>
    /// The collection of order rows for the selected store.
    /// </summary>
    [ObservableProperty]
    private ObservableCollection<StoreOrderRowDto> orderRows = new();

    /// <summary>
    /// The total price in BGN for all order rows.
    /// </summary>
    [ObservableProperty]
    private decimal totalBGN;

    /// <summary>
    /// The total price in Euro for all order rows.
    /// </summary>
    [ObservableProperty]
    private decimal totalEuro;

    /// <summary>
    /// All available parts for suggestions and validation.
    /// </summary>
    public ObservableCollection<Part> AllParts { get; } = new();

    private readonly AutoPartDbContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="StoresViewModel"/> class.
    /// Loads parts, sets up localization, and subscribes to order row changes.
    /// </summary>
    public StoresViewModel(AutoPartDbContext context)
    {
        _context = context;

        // Load all parts for suggestions/validation
        LoadParts();

        // Initialize stores list
        UpdateStores();

        // Subscribe to language change for localization
        WeakReferenceMessenger.Default.Register<LanguageChangedMessage>(this, (r, m) =>
        {
            UpdateStores();
        });

        // Subscribe to collection changes for totals and row property changes
        OrderRows.CollectionChanged += (s, e) =>
        {
            if (e.NewItems != null)
            {
                foreach (StoreOrderRowDto row in e.NewItems)
                {
                    row.AllParts = AllParts; // Ensure validation works for all rows
                    row.PropertyChanged += StoreOrderRow_PropertyChanged;
                }
            }
            RecalculateTotals();
        };
    }

    /// <summary>
    /// Handles property changes in order rows for auto-fill and totals.
    /// </summary>
    private void StoreOrderRow_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        var row = sender as StoreOrderRowDto;
        if (row == null) return;

        // Auto-fill Description and Prices when PartId changes
        if (e.PropertyName == nameof(StoreOrderRowDto.PartId))
        {
            var part = AllParts.FirstOrDefault(p => p.Id == row.PartId);
            if (part != null)
            {
                row.Description = part.Description;
                row.PriceBGN = part.PriceBGN;
                row.PriceEuro = part.PriceEURO;
            }
            else
            {
                row.Description = string.Empty;
                row.PriceBGN = 0;
                row.PriceEuro = 0;
            }
        }

        // Recalculate totals when relevant properties change
        if (e.PropertyName == nameof(StoreOrderRowDto.Quantity) ||
            e.PropertyName == nameof(StoreOrderRowDto.PriceBGN) ||
            e.PropertyName == nameof(StoreOrderRowDto.PriceEuro) ||
            e.PropertyName == nameof(StoreOrderRowDto.PartId))
        {
            RecalculateTotals();
        }
    }

    partial void OnOrderRowsChanged(ObservableCollection<StoreOrderRowDto> value)
    {
        if (value != null)
        {
            foreach (var row in value)
                row.PropertyChanged += StoreOrderRow_PropertyChanged;
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
        var row = new StoreOrderRowDto
        {
            AllParts = AllParts // Set reference for validation
        };
        row.PropertyChanged += StoreOrderRow_PropertyChanged;
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

    /// <summary>
    /// Loads all parts from the database for suggestions and validation.
    /// </summary>
    private void LoadParts()
    {
        AllParts.Clear();
        foreach (var part in _context.PartsInStock.AsNoTracking())
            AllParts.Add(part);
    }

    /// <summary>
    /// Updates the list of stores with localized names.
    /// </summary>
    private void UpdateStores()
    {
        Stores.Clear();
        Stores.Add(Properties.Strings.SofiaName);
        Stores.Add(Properties.Strings.PlovdivName);
    }

    #region Localization Properties
    public string StoresName => Properties.Strings.StoresName;
    /// <summary>Localized label for store selection.</summary>
    public string SelectStoreName => Properties.Strings.SelectStoreName;
    /// <summary>Localized header for Part ID.</summary>
    public string PartIDName => Properties.Strings.PartIDName;
    /// <summary>Localized header for Description.</summary>
    public string DescriptionName => Properties.Strings.DescriptionName;
    /// <summary>Localized header for Quantity.</summary>
    public string QuantityName => Properties.Strings.QuantityName;
    /// <summary>Localized header for Total BGN.</summary>
    public string TotalBGNName => Properties.Strings.TotalBGNName;
    /// <summary>Localized header for Total Euro.</summary>
    public string TotalEuroName => Properties.Strings.TotalEuroName;
    /// <summary>Localized label for total order price in BGN.</summary>
    public string TotalOrderPriceBGNName => Properties.Strings.TotalOrderPriceBGNName;
    /// <summary>Localized label for total order price in Euro.</summary>
    public string TotalOrderPriceEUROName => Properties.Strings.TotalOrderPriceEUROName;
    #endregion
}
