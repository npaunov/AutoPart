namespace AutoPartApp;

public class Part
{
    public int Id { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal PriceBGN { get; set; }
    public decimal PriceEURO => Math.Round(PriceBGN / 1.95583m, 2); // Conversion rate from BGN to EURO, rounded to 2 decimal places
    public int Package { get; set; } // Changed from string to int
    public int InStore { get; set; }
}
