namespace AutoPartApp.ViewModels;

public class MainViewModel
{
    public AutoPartsViewModel AutoPartsViewModel { get; } = new();
    public WarehouseViewModel WarehouseViewModel { get; } = new();
}