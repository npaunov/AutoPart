using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace AutoPartApp;

public class WarehouseViewModel : INotifyPropertyChanged
{
    private Warehouse _warehouse = new();

    public Warehouse Warehouse
    {
        get => _warehouse;
        set
        {
            _warehouse = value;
            OnPropertyChanged();
        }
    }

    public ICommand UpdateWarehouseCommand { get; }

    public WarehouseViewModel()
    {
        Warehouse.PartQuantities[1] = 100;
        Warehouse.PartQuantities[2] = 200;

        UpdateWarehouseCommand = new RelayCommand(UpdateWarehouse, CanUpdateWarehouse);
    }

    private void UpdateWarehouse()
    {
        // Example logic to update warehouse quantities
        if (Warehouse.PartQuantities.ContainsKey(1))
        {
            Warehouse.PartQuantities[1] += 10;
        }
    }

    private bool CanUpdateWarehouse() => true;

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
