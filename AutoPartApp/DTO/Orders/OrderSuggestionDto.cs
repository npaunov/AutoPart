using CommunityToolkit.Mvvm.ComponentModel;

namespace AutoPartApp
{
    /// <summary>
    /// Represents a suggested order row for the order planning DataGrid.
    /// Contains calculated and display properties for order planning.
    /// </summary>
    public partial class OrderSuggestionDto : ObservableObject
    {
        /// <summary>
        /// Gets or sets the unique identifier of the part.
        /// </summary>
        public string PartId { get; set; }

        /// <summary>
        /// Gets or sets the description of the part.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the package size for the part.
        /// </summary>
        public int Package { get; set; }

        private int _neededQty;
        /// <summary>
        /// Gets or sets the needed quantity to reach the target stock.
        /// Editable by the user.
        /// </summary>
        public int NeededQty
        {
            get => _neededQty;
            set
            {
                SetProperty(ref _neededQty, value);
                UpdateOrderQty();
            }
        }

        private int _orderQty;
        /// <summary>
        /// Gets the quantity to be ordered, rounded up to the nearest package.
        /// </summary>
        public int OrderQty
        {
            get => _orderQty;
            private set => SetProperty(ref _orderQty, value);
        }

        /// <summary>
        /// Gets or sets the price per unit in BGN.
        /// </summary>
        public decimal PriceBGN { get; set; }

        /// <summary>
        /// Gets or sets the price per unit in EUR.
        /// </summary>
        public decimal PriceEURO { get; set; }

        /// <summary>
        /// Gets the total price in BGN for the order quantity.
        /// </summary>
        public decimal TotalBGN => OrderQty * PriceBGN;

        /// <summary>
        /// Gets the total price in EUR for the order quantity.
        /// </summary>
        public decimal TotalEURO => OrderQty * PriceEURO;

        /// <summary>
        /// Gets or sets the sales quantity for the current month, 1 year ago.
        /// </summary>
        public int Sales1 { get; set; }
        /// <summary>
        /// Gets or sets the sales quantity for the next month, 1 year ago.
        /// </summary>
        public int Sales2 { get; set; }
        /// <summary>
        /// Gets or sets the sales quantity for the current month, 2 years ago.
        /// </summary>
        public int Sales3 { get; set; }
        /// <summary>
        /// Gets or sets the sales quantity for the next month, 2 years ago.
        /// </summary>
        public int Sales4 { get; set; }
        /// <summary>
        /// Gets or sets the sales quantity for the current month, 3 years ago.
        /// </summary>
        public int Sales5 { get; set; }
        /// <summary>
        /// Gets or sets the sales quantity for the next month, 3 years ago.
        /// </summary>
        public int Sales6 { get; set; }

        /// <summary>
        /// Updates the order quantity based on the needed quantity and package size.
        /// </summary>
        private void UpdateOrderQty()
        {
            // Always round up to the next package
            OrderQty = ((NeededQty + Package - 1) / Package) * Package;
            OnPropertyChanged(nameof(TotalBGN));
            OnPropertyChanged(nameof(TotalEURO));
        }
    }
}