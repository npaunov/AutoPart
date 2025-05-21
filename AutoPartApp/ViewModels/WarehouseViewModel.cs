using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using AutoPartApp.Models;
using AutoPartApp.Utilities;
using Microsoft.EntityFrameworkCore;
using AutoPartApp.EntityFramework;

namespace AutoPartApp;

public partial class WarehouseViewModel : ObservableObject
{
    [ObservableProperty]
    private string _searchPartId = string.Empty;

    private List<Part> _allParts = new();
    public Warehouse Warehouse { get; set; } = new();

    private readonly AutoPartDbContext _context;

    public WarehouseViewModel(AutoPartDbContext context)
    {
        _context = context;
        LoadFromDatabase();
    }

    [RelayCommand]
    /// <summary>
    /// Filters the parts based on the entered part ID.
    /// </summary>
    private void Search()
    {
        Warehouse.PartsInStock.Clear();

        IEnumerable<Part> results;
        if (!string.IsNullOrWhiteSpace(SearchPartId))
        {
            string search = SearchPartId.ToLower();
            results = _allParts.Where(p =>
                (!string.IsNullOrEmpty(p.Id) && p.Id.ToLower().Contains(search)) ||
                (!string.IsNullOrEmpty(p.Description) && p.Description.ToLower().Contains(search)));
        }
        else
        {
            results = _allParts;
        }

        foreach (var part in results)
            Warehouse.PartsInStock.Add(part);
    }

    [RelayCommand]
    private void Clear()
    {
        // write the logic to clear the search
        SearchPartId = string.Empty;
        Warehouse.PartsInStock.Clear();
        foreach (var part in _allParts)
            Warehouse.PartsInStock.Add(part);
    }

    /// <summary>
    /// Loads parts from the last imported CSV file into the warehouse for preview purposes.
    /// This does not affect the database; it only updates the in-memory collection for UI display.
    /// </summary>
    public void LoadFromCsv()
    {
        Warehouse.PartsInStock.Clear();
        foreach (var part in DataImportUtil.ImportedParts)
            Warehouse.PartsInStock.Add(part);
        _allParts = DataImportUtil.ImportedParts.ToList();
    }

    /// <summary>
    /// Loads parts from the database into the warehouse, ensuring the UI reflects the current persisted data.
    /// This should be called after any operation that modifies the database.
    /// </summary>
    public void LoadFromDatabase()
    {
        var dbParts = _context.PartsInStock.AsNoTracking().ToList();
        Warehouse.PartsInStock.Clear();
        foreach (var part in dbParts)
            Warehouse.PartsInStock.Add(part);
        _allParts = dbParts;
    }

    // Localized Name and Label Properties
    public string PartIdHeader => Properties.Strings.PartIDName;
    public string DescriptionHeader => Properties.Strings.DescriptionName;
    public string PriceBGNHeader => Properties.Strings.PriceName + " " + Properties.Strings.BGNName;
    public string PriceEUROHeader => Properties.Strings.PriceName + " " + Properties.Strings.EuroName;
    public string PackageHeader => Properties.Strings.PackageName;
    public string InStoreHeader => Properties.Strings.InStoreName;
    public string SearchPartIdLabel => Properties.Strings.SearchLabelName;
    public string SearchButtonLabel => Properties.Strings.SearchName;
    public string ClearNameLabel => Properties.Strings.ClearName;
}
