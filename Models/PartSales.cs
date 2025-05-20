namespace AutoPartApp.Models
{
    /// <summary>
    /// Represents the sales data for a specific part.
    /// </summary>
    public class PartSales
    {
        /// <summary>
        /// Gets or sets the unique identifier for the part.
        /// </summary>
        public string PartId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the total number of sales for the part.
        /// </summary>
        public int TotalSales { get; set; }

        /// <summary>
        /// Gets or sets the list of dates when the part was sold.
        /// </summary>
        public List<DateTime> SalesDates { get; set; } = new();
    }
}