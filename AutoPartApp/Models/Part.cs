namespace AutoPartApp;

public class Part
{
    public int Id { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal PriceBGN { get; set; }
    public decimal PriceEURO => PriceBGN / 1.95583m; // Conversion rate from BGN to EURO
    public int Package { get; set; } // Changed from string to int
    public int InStore { get; set; }
}