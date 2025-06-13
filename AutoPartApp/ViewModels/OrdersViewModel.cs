using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using AutoPartApp.BusinessLogic.OrdersLogic;
using AutoPartApp.DIServices.Services.Interfaces;
using AutoPartApp.DTO.OrdersDTO;
using AutoPart.DataAccess;

namespace AutoPartApp.ViewModels;

/// <summary>
/// ViewModel for the Orders view, providing order planning and suggestion logic.
/// </summary>
public partial class OrdersViewModel : ObservableObject
{
    private readonly AutoPartDbContext _context;
    private readonly IDialogService _dialogService;

    /// <summary>
    /// Gets or sets the collection of order suggestions for display in the DataGrid.
    /// </summary>
    [ObservableProperty]
    private ObservableCollection<OrderSuggestionDto> orderSuggestions = new();

    private int _monthsToOrder = 2;

    /// <summary>
    /// Gets the total price in BGN for all suggested orders.
    /// </summary>
    [ObservableProperty]
    private decimal totalBGN;

    /// <summary>
    /// Gets the total price in EUR for all suggested orders.
    /// </summary>
    [ObservableProperty]
    private decimal totalEURO;

    /// <summary>
    /// Gets or sets the number of months to order for. Default is 2.
    /// Shows a warning if set above 2, and prevents values above 12.
    /// </summary>
    public int MonthsToOrder
    {
        get => _monthsToOrder;
        set
        {
            if (value > 12)
            {
                _dialogService.ShowMessage(
                    "Months to order cannot be greater than 12. Value will be set to 12.",
                    "Warning");
                value = 12;
            }
            else if (value > 2)
            {
                _dialogService.ShowMessage(
                    "Warning: Ordering for more than 2 months may result in overstock.",
                    "Warning");
            }
            SetProperty(ref _monthsToOrder, value);
        }
    }

    #region Sales Headers
    /// <summary>
    /// Gets the header for the first sales column (last month, format "yy-M").
    /// </summary>
    public string Sales1Header { get; private set; }

    /// <summary>
    /// Gets the header for the second sales column (same month last year, format "yy-M").
    /// </summary>
    public string Sales2Header { get; private set; }

    /// <summary>
    /// Gets the header for the third sales column (next month last year, format "yy-M").
    /// </summary>
    public string Sales3Header { get; private set; }

    /// <summary>
    /// Gets the header for the fourth sales column (same month two years ago, format "yy-M").
    /// </summary>
    public string Sales4Header { get; private set; }

    /// <summary>
    /// Gets the header for the fifth sales column (next month two years ago, format "yy-M").
    /// </summary>
    public string Sales5Header { get; private set; }
    #endregion

    /// <summary>
    /// Initializes a new instance of the <see cref="OrdersViewModel"/> class.
    /// </summary>
    /// <param name="context">The application's database context.</param>
    /// <param name="dialogService">The dialog service for showing messages.</param>
    public OrdersViewModel(AutoPartDbContext context, IDialogService dialogService)
    {
        _context = context;
        _dialogService = dialogService;
        MonthsToOrder = 2;
        OrderSuggestions = new ObservableCollection<OrderSuggestionDto>();
        UpdateSalesHeaders(DateTime.Now);
    }

    /// <summary>
    /// Generates order suggestions based on current inventory and sales history.
    /// </summary>
    [RelayCommand]
    public void GenerateOrderSuggestions()
    {
        // Validate months to order (enforce lower bound)
        if (MonthsToOrder < 1)
            MonthsToOrder = 1;

        UpdateSalesHeaders(DateTime.Now);

        var parts = _context.PartsInStock.ToList();
        var sales = _context.PartSales.ToList();

        var suggestions = OrdersLogic.GetOrderSuggestions(parts, sales, MonthsToOrder);

        OrderSuggestions = new ObservableCollection<OrderSuggestionDto>(suggestions);

        // Subscribe to NeededQty property changes to recalculate totals
        foreach (var suggestion in OrderSuggestions)
        {
            suggestion.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(OrderSuggestionDto.NeededQty) ||
                    e.PropertyName == nameof(OrderSuggestionDto.OrderQty))
                {
                    RecalculateTotals();
                }
            };
        }

        RecalculateTotals();
    }

    /// <summary>
    /// Updates the sales column header properties based on the given reference date.
    /// Each header is set to a "yy-M" string representing the corresponding month and year:
    /// - Sales1Header: last month
    /// - Sales2Header: same month last year
    /// - Sales3Header: next month last year
    /// - Sales4Header: same month two years ago
    /// - Sales5Header: next month two years ago
    /// Notifies property changes for all header properties.
    /// </summary>
    /// <param name="referenceDate">The date to use as the current reference for calculating headers.</param>
    private void UpdateSalesHeaders(DateTime referenceDate)
    {
        // Last month
        Sales1Header = referenceDate.AddMonths(-1).ToString("yy-M");
        // Same month last year
        Sales2Header = referenceDate.AddYears(-1).ToString("yy-M");
        // Next month last year
        Sales3Header = referenceDate.AddYears(-1).AddMonths(1).ToString("yy-M");
        // Same month two years ago
        Sales4Header = referenceDate.AddYears(-2).ToString("yy-M");
        // Next month two years ago
        Sales5Header = referenceDate.AddYears(-2).AddMonths(1).ToString("yy-M");

        OnPropertyChanged(nameof(Sales1Header));
        OnPropertyChanged(nameof(Sales2Header));
        OnPropertyChanged(nameof(Sales3Header));
        OnPropertyChanged(nameof(Sales4Header));
        OnPropertyChanged(nameof(Sales5Header));
    }

    /// <summary>
    /// Recalculates the total BGN and EUR prices for all suggested orders.
    /// </summary>
    private void RecalculateTotals()
    {
        TotalBGN = OrderSuggestions.Sum(x => x.TotalBGN);
        TotalEURO = OrderSuggestions.Sum(x => x.TotalEURO);
    }

    #region Localized Properties
    public string OrdersName => Properties.Strings.OrdersName;
    public string MonthsToOrderName => Properties.Strings.MonthsToOrderName;
    public string SuggestOrdersName => Properties.Strings.SuggestOrdersName;
    public string TotalOrderPriceBGNName => Properties.Strings.TotalOrderPriceBGNName;
    public string TotalOrderPriceEUROName => Properties.Strings.TotalOrderPriceEUROName;
    public string PartIDName => Properties.Strings.PartIDName;
    public string DescriptionName => Properties.Strings.DescriptionName;
    public string InStoreName => Properties.Strings.InStoreName;
    public string PackageName => Properties.Strings.PackageName;
    public string NeededQtyName => Properties.Strings.NeededQtyName;
    public string OrderQtyName => Properties.Strings.OrderQtyName;
    public string TotalBGNName => Properties.Strings.TotalBGNName;
    public string TotalEuroName => Properties.Strings.TotalEuroName;
    #endregion

}