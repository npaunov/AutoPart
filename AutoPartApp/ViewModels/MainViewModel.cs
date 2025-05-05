namespace AutoPartApp;

/// <summary>
/// The main ViewModel for the application.
/// </summary>
public class MainViewModel
{
    public AutoPartsViewModel AutoPartsViewModel { get; } = new();
    public WarehouseViewModel WarehouseViewModel { get; } = new();
}