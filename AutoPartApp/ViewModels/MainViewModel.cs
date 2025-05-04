using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using AutoPartApp.Models;

namespace AutoPartApp.ViewModels;

public class MainViewModel : INotifyPropertyChanged
{
    private Part? _selectedPart;
    private Warehouse _warehouse = new();

    public ObservableCollection<Part> Parts { get; set; } = new();

    public Part? SelectedPart
    {
        get => _selectedPart;
        set
        {
            _selectedPart = value;
            OnPropertyChanged();
        }
    }

    public Warehouse Warehouse
    {
        get => _warehouse;
        set
        {
            _warehouse = value;
            OnPropertyChanged();
        }
    }

    public MainViewModel()
    {
        // Sample data
        Parts.Add(new Part { Id = 1, Description = "Brake Pad", Price = 29.99m, SalesForYear = 500 });
        Parts.Add(new Part { Id = 2, Description = "Oil Filter", Price = 15.49m, SalesForYear = 1200 });
        Parts.Add(new Part { Id = 3, Description = "Air Filter", Price = 19.99m, SalesForYear = 800 });

        Warehouse.PartQuantities[1] = 100;
        Warehouse.PartQuantities[2] = 200;
        Warehouse.PartQuantities[3] = 150;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
