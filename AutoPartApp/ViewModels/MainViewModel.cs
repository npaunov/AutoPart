using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace AutoPartApp;

/// <summary>
/// The main ViewModel for the application.
/// Manages the data and commands for Auto Parts and Warehouse.
/// </summary>
public class MainViewModel : INotifyPropertyChanged
{
    private Part? _selectedPart;
    private Warehouse _warehouse = new();

    /// <summary>
    /// Gets or sets the collection of auto parts.
    /// </summary>
    public ObservableCollection<Part> Parts { get; set; } = new();

    /// <summary>
    /// Gets or sets the currently selected part.
    /// </summary>
    public Part? SelectedPart
    {
        get => _selectedPart;
        set
        {
            _selectedPart = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// Gets or sets the warehouse data.
    /// </summary>
    public Warehouse Warehouse
    {
        get => _warehouse;
        set
        {
            _warehouse = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// Command to add a new part to the collection.
    /// </summary>
    public ICommand AddPartCommand { get; }

    /// <summary>
    /// Command to remove the currently selected part.
    /// </summary>
    public ICommand RemovePartCommand { get; }

    /// <summary>
    /// Command to update the warehouse quantities for the selected part.
    /// </summary>
    public ICommand UpdateWarehouseCommand { get; }

    /// <summary>
    /// Initializes a new instance of the MainViewModel class.
    /// </summary>
    public MainViewModel()
    {
        // Initialize sample data
        Parts.Add(new Part { Id = 1, Description = "Brake Pad", Price = 29.99m, SalesForYear = 500 });
        Parts.Add(new Part { Id = 2, Description = "Oil Filter", Price = 15.49m, SalesForYear = 1200 });
        Parts.Add(new Part { Id = 3, Description = "Air Filter", Price = 19.99m, SalesForYear = 800 });

        Warehouse.PartQuantities[1] = 100;
        Warehouse.PartQuantities[2] = 200;
        Warehouse.PartQuantities[3] = 150;

        // Initialize commands
        AddPartCommand = new RelayCommand(AddPart);
        RemovePartCommand = new RelayCommand(RemovePart, CanRemovePart);
        UpdateWarehouseCommand = new RelayCommand(UpdateWarehouse, CanUpdateWarehouse);
    }

    /// <summary>
    /// Adds a new part to the collection.
    /// </summary>
    private void AddPart()
    {
        var newPart = new Part
        {
            Id = Parts.Count + 1,
            Description = "New Part",
            Price = 0.0m,
            SalesForYear = 0
        };
        Parts.Add(newPart);
    }

    /// <summary>
    /// Removes the currently selected part from the collection.
    /// </summary>
    private void RemovePart()
    {
        if (SelectedPart != null)
        {
            Parts.Remove(SelectedPart);
            SelectedPart = null;
        }
    }

    /// <summary>
    /// Determines whether the RemovePartCommand can execute.
    /// </summary>
    /// <returns>True if a part is selected; otherwise, false.</returns>
    private bool CanRemovePart()
    {
        return SelectedPart != null;
    }

    /// <summary>
    /// Updates the warehouse quantities for the selected part.
    /// </summary>
    private void UpdateWarehouse()
    {
        if (SelectedPart != null)
        {
            if (Warehouse.PartQuantities.ContainsKey(SelectedPart.Id))
            {
                Warehouse.PartQuantities[SelectedPart.Id] += 10; // Example: Add 10 units
            }
            else
            {
                Warehouse.PartQuantities[SelectedPart.Id] = 10;
            }
        }
    }

    /// <summary>
    /// Determines whether the UpdateWarehouseCommand can execute.
    /// </summary>
    /// <returns>True if a part is selected; otherwise, false.</returns>
    private bool CanUpdateWarehouse()
    {
        return SelectedPart != null;
    }

    /// <summary>
    /// Occurs when a property value changes.
    /// </summary>
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    /// Notifies listeners that a property value has changed.
    /// </summary>
    /// <param name="propertyName">The name of the property that changed.</param>
    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

