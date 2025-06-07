using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.ComponentModel;
using AutoPartApp.Models;

namespace AutoPartApp.DTO.Orders;

/// <summary>
/// Represents a single row in the store order grid, including validation for PartId and Quantity.
/// </summary>
public partial class StoreOrderRowDto : ObservableObject, IDataErrorInfo
{
    /// <summary>
    /// Row number in the grid.
    /// </summary>
    [ObservableProperty]
    private int rowNumber;

    private string _partId;
    /// <summary>
    /// Part identifier. Must exist in <see cref="AllParts"/> to be valid.
    /// If set to an invalid value or null, Quantity is reset to 0.
    /// </summary>
    public string PartId
    {
        get => _partId;
        set
        {
            if (SetProperty(ref _partId, value))
            {
                // Reset Quantity to 0 if PartId is invalid or null
                if (string.IsNullOrWhiteSpace(_partId) || AllParts == null || !AllParts.Any(p => p.Id == _partId))
                {
                    Quantity = 0;
                }
            }
        }
    }

    private string _description;
    /// <summary>
    /// Part description.
    /// </summary>
    public string Description
    {
        get => _description;
        set => SetProperty(ref _description, value);
    }

    private int _quantity;
    /// <summary>
    /// Ordered quantity. Must be greater than 0.
    /// </summary>
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
    /// <summary>
    /// Unit price in BGN.
    /// </summary>
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
    /// <summary>
    /// Unit price in Euro.
    /// </summary>
    public decimal PriceEuro
    {
        get => _priceEuro;
        set
        {
            if (SetProperty(ref _priceEuro, value))
                OnPropertyChanged(nameof(TotalEuro));
        }
    }

    /// <summary>
    /// Total price for this row in BGN.
    /// </summary>
    public decimal TotalBGN => Quantity * PriceBGN;

    /// <summary>
    /// Total price for this row in Euro.
    /// </summary>
    public decimal TotalEuro => Quantity * PriceEuro;

    /// <summary>
    /// Reference to all available parts for validation. Set by the ViewModel.
    /// </summary>
    public ObservableCollection<Part> AllParts { get; set; }

    /// <summary>
    /// Validation logic for DataGrid. Returns an error if PartId or Quantity is not valid.
    /// </summary>
    public string this[string columnName]
    {
        get
        {
            if (columnName == nameof(PartId))
            {
                if (string.IsNullOrWhiteSpace(PartId) || AllParts == null || !AllParts.Any(p => p.Id == PartId))
                    return Properties.Strings.InvalidPartIdMessage;
            }
            if (columnName == nameof(Quantity))
            {
                if (Quantity <= 0)
                    return Properties.Strings.InvalidQuantityMessage; // e.g. "Quantity must be greater than 0 and less than or equal to 10,000."
                if (Quantity > 10000)
                    return Properties.Strings.InvalidQuantityMessage; // Use the same or a new message, e.g. "Quantity must not exceed 10,000."
            }
            return null;
        }
    }

    /// <summary>
    /// Not used. Required by IDataErrorInfo.
    /// </summary>
    public string Error => null;
}
