using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace AutoPartApp;

/// <summary>
/// ViewModel for the Stores tab, managing store selection and order entry.
/// </summary>
public partial class StoresViewModel : ObservableObject
{
    /// <summary>List of available stores.</summary>
    public ObservableCollection<string> Stores { get; } = new() { "Sofia", "Plovdiv" };

    [ObservableProperty]
    private string selectedStore;

    [ObservableProperty]
    private ObservableCollection<StoreOrderRowDto> orderRows = new();

    [ObservableProperty]
    private decimal totalBGN;

    [ObservableProperty]
    private decimal totalEuro;

    /// <summary>
    /// Initializes a new instance of the <see cref="StoresViewModel"/> class.
    /// </summary>
    public StoresViewModel()
    {
        OrderRows.CollectionChanged += (_, _) => RecalculateTotals();
    }

    partial void OnOrderRowsChanged(ObservableCollection<StoreOrderRowDto> value)
    {
        if (value != null)
        {
            foreach (var row in value)
                row.PropertyChanged += (_, __) => RecalculateTotals();
        }
        RecalculateTotals();
    }

    partial void OnSelectedStoreChanged(string value)
    {
        OrderRows.Clear();
        RecalculateTotals();
    }

    /// <summary>
    /// Adds an empty row to the order grid.
    /// </summary>
    public void AddEmptyRow()
    {
        var row = new StoreOrderRowDto();
        row.PropertyChanged += (_, __) => RecalculateTotals();
        OrderRows.Add(row);
    }

    /// <summary>
    /// Recalculates the total prices for all order rows.
    /// </summary>
    private void RecalculateTotals()
    {
        TotalBGN = OrderRows.Sum(r => r.TotalBGN);
        TotalEuro = OrderRows.Sum(r => r.TotalEuro);
    }
}
