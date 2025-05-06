using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

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
        // Initialize the warehouse with sample data
        Warehouse.Parts.Add(new Part { Id = 1, Description = "Engine", PriceBGN = 500, Package = 1, InStore = 10 });
        Warehouse.Parts.Add(new Part { Id = 2, Description = "Gearbox", PriceBGN = 300, Package = 2, InStore = 5 });
        Warehouse.Parts.Add(new Part { Id = 3, Description = "Tire", PriceBGN = 100, Package = 3, InStore = 50 });

        // Initialize the filtered parts with all parts
        FilteredParts = new ObservableCollection<Part>(Warehouse.Parts);

        // Initialize the search command
        SearchCommand = new RelayCommand(Search);
    }

    private void Search()
    {
        if (int.TryParse(SearchPartId, out int partId))
        {
            // Filter parts by the entered part ID
            FilteredParts = new ObservableCollection<Part>(Warehouse.Parts.Where(p => p.Id == partId));
        }
        else
        {
            // If no valid part ID is entered, show all parts
            FilteredParts = new ObservableCollection<Part>(Warehouse.Parts);
        }
    }
}
