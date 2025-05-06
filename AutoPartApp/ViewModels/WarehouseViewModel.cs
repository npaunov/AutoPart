using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using AutoPartApp.Services;

namespace AutoPartApp;

public class WarehouseViewModel : BaseViewModel
{
    private string _searchPartId = string.Empty;
    private ObservableCollection<Part> _filteredParts = new();

    public Warehouse Warehouse { get; set; } = new();

    public string SearchPartId
    {
        get => _searchPartId;
        set
        {
            if (_searchPartId != value)
            {
                _searchPartId = value;
                OnPropertyChanged();
            }
        }
    }

    public ObservableCollection<Part> FilteredParts
    {
        get => _filteredParts;
        set
        {
            _filteredParts = value;
            OnPropertyChanged();
        }
    }

    public ICommand SearchCommand { get; }
    public ICommand LoadImportedPartsCommand { get; }

    // Localized Name and Label Properties
    public string PartIdHeader => Properties.Strings.PartIDName;
    public string DescriptionHeader => Properties.Strings.DescriptionName;
    public string PriceBGNHeader => Properties.Strings.PriceName + " " + Properties.Strings.BGNName;
    public string PriceEUROHeader => Properties.Strings.PriceName + " " + Properties.Strings.EuroName;
    public string PackageHeader => Properties.Strings.PackageName;
    public string InStoreHeader => Properties.Strings.InStoreName;
    public string SearchPartIdLabel => Properties.Strings.SearchName + " " + Properties.Strings.PartIDName;
    public string SearchButtonLabel => Properties.Strings.SearchName;

    public WarehouseViewModel()
    {
        // Initialize the search command
        SearchCommand = new RelayCommand(Search);

        // Initialize the load imported parts command
        LoadImportedPartsCommand = new RelayCommand(LoadImportedParts);

        // Load the imported parts initially (if any)
        LoadImportedParts();
    }

    /// <summary>
    /// Filters the parts based on the entered part ID.
    /// </summary>
    private void Search()
    {
        if (!string.IsNullOrEmpty(SearchPartId))
        {
            // Filter parts by the entered part ID
            FilteredParts = new ObservableCollection<Part>(Warehouse.Parts.Where(p => p.Id.Equals(SearchPartId)));
        }
        else
        {
            // If no valid part ID is entered, show all parts
            FilteredParts = Warehouse.Parts;
        }
    }

    /// <summary>
    /// Loads the parts imported by the DataImportService into the Warehouse and updates the FilteredParts collection.
    /// </summary>
    public void LoadImportedParts()
    {
        // Clear the current warehouse parts and load the imported parts
        //Warehouse.Parts.Clear();
        Warehouse.Parts = DataImportService.ImportedParts;

        // Update the filtered parts to reflect the imported data
        FilteredParts = DataImportService.ImportedParts;
    }
}
