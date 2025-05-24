using CommunityToolkit.Mvvm.ComponentModel;

namespace AutoPartApp;

/// <summary>
/// Represents a single row in the store order grid.
/// </summary>
public class StoreOrderRowDto : ObservableObject
{
    /// <summary>Row number in the grid.</summary>
    public int RowNumber { get; set; }

    private string _partId;
    /// <summary>Part identifier.</summary>
    public string PartId
    {
        get => _partId;
        set => SetProperty(ref _partId, value);
    }

    private string _description;
    /// <summary>Part description.</summary>
    public string Description
    {
        get => _description;
        set => SetProperty(ref _description, value);
    }

    private int _quantity;
    /// <summary>Ordered quantity.</summary>
    public int Quantity
    {
        get => _quantity;
        set
        {
            if (SetProperty(ref _quantity, value))
            {
                OnPropertyChanged(nameof(TotalBGN));
                OnPropertyChanged(nameof(TotalEuro));
            }
        }
    }

    private decimal _priceBGN;
    /// <summary>Unit price in BGN.</summary>
    public decimal PriceBGN
    {
        get => _priceBGN;
        set
        {
            if (SetProperty(ref _priceBGN, value))
                OnPropertyChanged(nameof(TotalBGN));
        }
    }

    private decimal _priceEuro;
    /// <summary>Unit price in Euro.</summary>
    public decimal PriceEuro
    {
        get => _priceEuro;
        set
        {
            if (SetProperty(ref _priceEuro, value))
                OnPropertyChanged(nameof(TotalEuro));
        }
    }

    /// <summary>Total price for this row in BGN.</summary>
    public decimal TotalBGN => Quantity * PriceBGN;

    /// <summary>Total price for this row in Euro.</summary>
    public decimal TotalEuro => Quantity * PriceEuro;
}
