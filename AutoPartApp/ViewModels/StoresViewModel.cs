using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.ComponentModel;
using AutoPart.DataAccess;
using AutoPart.Models;
using AutoPartApp.DIServices.Messengers;
using AutoPartApp.DTO.Orders;

namespace AutoPartApp.ViewModels;

/// <summary>
/// ViewModel for the Stores tab, managing store selection, order entry, and part suggestions/validation.
/// </summary>
public partial class StoresViewModel : ObservableObject
{
    #region ObservableProperties
    /// <summary>
    /// List of available stores.
    /// </summary>
    [ObservableProperty]
    private ObservableCollection<StoreDto> _stores = new();

    /// <summary>
    /// The currently selected store.
    /// </summary>
    [ObservableProperty]
    private StoreDto selectedStore;

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
    /// Gets or sets the currently selected order row in the grid.
    /// </summary>
    [ObservableProperty]
    private StoreOrderRowDto selectedOrderRow;
    #endregion 

    private string? _cachedSelectedStoreKey;
    private List<StoreOrderRowDto>? _cachedOrderRows;

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
            CacheState();
            UpdateStores();
            RestoreState();
        });

        // Subscribe to collection changes for totals and row property changes
        OrderRows.CollectionChanged += OrderRows_CollectionChanged;

        // Ensure the grid starts with one empty row
        if (OrderRows.Count == 0)
            AddEmptyRow();
    }

    #region Commands
    /// <summary>
    /// Adds an empty row to the order grid only if the last row is valid or the grid is empty.
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanAddEmptyRow))]
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
    /// Determines whether a new row can be added based on the validity of the last row.
    /// </summary>
    private bool CanAddEmptyRow()
    {
        if (OrderRows.Count == 0)
            return true;

        var lastRow = OrderRows.Last();

        // Validate PartId
        bool validPart = !string.IsNullOrWhiteSpace(lastRow.PartId)
            && AllParts.Any(p => p.Id == lastRow.PartId);

        // Validate Quantity
        bool validQuantity = lastRow.Quantity > 0 && lastRow.Quantity <= 10000;

        return validPart && validQuantity;
    }

    /// <summary>
    /// Deletes the currently selected order row from the collection, if any.
    /// Does nothing if no row is selected.
    /// </summary>
    [RelayCommand]
    private void DeleteSelectedRow()
    {
        if (SelectedOrderRow != null)
        {
            OrderRows.Remove(SelectedOrderRow);
            UpdateRowNumbers();
        }
    }
    #endregion

    /// <summary>
    /// Handles changes to the OrderRows collection, such as adding or removing rows.
    /// Updates row numbers and recalculates totals.
    /// </summary>
    private void OrderRows_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        if (e.NewItems != null)
        {
            foreach (StoreOrderRowDto row in e.NewItems)
            {
                row.AllParts = AllParts; // Ensure validation works for all rows
                row.PropertyChanged += StoreOrderRow_PropertyChanged;
            }
        }
        UpdateRowNumbers();
        RecalculateTotals();
        AddEmptyRowCommand.NotifyCanExecuteChanged();
    }

    #region Handlers for Property Changes
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

        // Notify command system if the changed row is the last row
        if (OrderRows.Count > 0 && row == OrderRows.Last() &&
            (e.PropertyName == nameof(StoreOrderRowDto.PartId) || e.PropertyName == nameof(StoreOrderRowDto.Quantity)))
        {
            AddEmptyRowCommand.NotifyCanExecuteChanged();
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

    partial void OnSelectedStoreChanged(StoreDto value)
    {
        OrderRows.Clear();
        AddEmptyRow(); // Always add one empty row after clearing
        RecalculateTotals();
    }
    #endregion

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
    /// Updates the list of stores with their keys and localized names.
    /// </summary>
    private void UpdateStores()
    {
        Stores.Clear();
        Stores.Add(new StoreDto { Key = "Sofia", DisplayName = Properties.Strings.SofiaName });
        Stores.Add(new StoreDto { Key = "Plovdiv", DisplayName = Properties.Strings.PlovdivName });
        // Add more stores
    }

    /// <summary>
    /// Updates the RowNumber property for all order rows to match their position in the collection.
    /// </summary>
    private void UpdateRowNumbers()
    {
        for (int i = 0; i < OrderRows.Count; i++)
        {
            OrderRows[i].RowNumber = i + 1;
        }
    }

    /// <summary>
    /// Caches the currently selected store key and order rows.
    /// </summary>
    private void CacheState()
    {
        _cachedSelectedStoreKey = SelectedStore?.Key;
        _cachedOrderRows = OrderRows.Select(row => new StoreOrderRowDto
        {
            PartId = row.PartId,
            Description = row.Description,
            Quantity = row.Quantity,
            PriceBGN = row.PriceBGN,
            PriceEuro = row.PriceEuro,
            AllParts = AllParts
        }).ToList();
    }

    /// <summary>
    /// Restores the selected store and order rows after a UI refresh (e.g., language change).
    /// </summary>
    private void RestoreState()
    {
        if (_cachedSelectedStoreKey != null)
            SelectedStore = Stores.FirstOrDefault(s => s.Key == _cachedSelectedStoreKey);

        if (_cachedOrderRows != null)
        {
            OrderRows.Clear();
            foreach (var row in _cachedOrderRows)
            {
                row.AllParts = AllParts;
                row.PropertyChanged += StoreOrderRow_PropertyChanged;
                OrderRows.Add(row);
            }
            UpdateRowNumbers();
            RecalculateTotals();
        }
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
    public string AddRowName => Properties.Strings.AddRowName;
    public string DeleteRowName => Properties.Strings.DeleteRowName;
    /// <summary>Localized label for total order price in BGN.</summary>
    public string TotalOrderPriceBGNName => Properties.Strings.TotalOrderPriceBGNName;
    /// <summary>Localized label for total order price in Euro.</summary>
    public string TotalOrderPriceEUROName => Properties.Strings.TotalOrderPriceEUROName;
    #endregion
}
