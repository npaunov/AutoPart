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
        LoadDataFromDatabase();
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

    /// <summary>
    /// Loads the parts imported by the DataImportUtil into the Warehouse and updates the FilteredParts collection.
    /// </summary>
    public void LoadImportedParts()
    {
        // Clear the current warehouse parts and add the imported parts
        Warehouse.PartsInStock.Clear();
        foreach (var part in DataImportUtil.ImportedParts)
        {
            Warehouse.PartsInStock.Add(part);
        }
    }

    public void LoadDataFromDatabase()
    {
        // Clear and reload the collection from the database
        _allParts = _context.PartsInStock.ToList();
        Warehouse.PartsInStock.Clear();
        Warehouse.PartsInStock = _allParts.ToObservableCollection();
    }

    // Localized Name and Label Properties
    public string PartIdHeader => Properties.Strings.PartIDName;
    public string DescriptionHeader => Properties.Strings.DescriptionName;
    public string PriceBGNHeader => Properties.Strings.PriceName + " " + Properties.Strings.BGNName;
    public string PriceEUROHeader => Properties.Strings.PriceName + " " + Properties.Strings.EuroName;
    public string PackageHeader => Properties.Strings.PackageName;
    public string InStoreHeader => Properties.Strings.InStoreName;
    public string SearchPartIdLabel => Properties.Strings.SearchName + " " + Properties.Strings.PartIDName;
    public string SearchButtonLabel => Properties.Strings.SearchName;
}
