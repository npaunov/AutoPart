using System.ComponentModel;
using System.Runtime.CompilerServices;

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

    public WarehouseViewModel()
    {
        Warehouse.PartQuantities[1] = 100;
        Warehouse.PartQuantities[2] = 200;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
