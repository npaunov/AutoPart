using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using AutoPartApp.EntityFramework;
using System.Linq;

namespace AutoPartApp;

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
    /// Recalculates the total BGN and EUR prices for all suggested orders.
    /// </summary>
    private void RecalculateTotals()
    {
        TotalBGN = OrderSuggestions.Sum(x => x.TotalBGN);
        TotalEURO = OrderSuggestions.Sum(x => x.TotalEURO);
    }

    public string OrdersName => Properties.Strings.OrdersName;
}