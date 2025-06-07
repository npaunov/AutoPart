using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using AutoPart.DataAccess;

namespace AutoPartApp.ViewModels;

public partial class SalesHistoryViewModel : ObservableObject
{
    public ObservableCollection<SalesHistoryRow> SalesRows { get; } = new();
    public List<string> MonthHeaders { get; } = new();
        private readonly AutoPartDbContext _context;

    public SalesHistoryViewModel(AutoPartDbContext context)
    {
        _context = context;
        LoadSalesHistory();
    }
    public void LoadSalesHistory()
    {
        SalesRows.Clear();

        // Generate month headers, descending from now
        var months = new List<string>();
        var now = new DateTime(2025, 6, 1); // or DateTime.Now for dynamic
        for (int i = 0; i < 36; i++)
        {
            months.Add(now.ToString("yy-MM"));
            now = now.AddMonths(-1);
        }
        MonthHeaders.Clear();
        MonthHeaders.AddRange(months);

        var parts = _context.PartsInStock.ToList();
        var sales = _context.PartSales.ToList();

        foreach (var part in parts)
        {
            var row = new SalesHistoryRow
            {
                PartId = part.Id,
                Description = part.Description,
                TotalSales = sales.Where(s => s.PartId == part.Id).Sum(s => s.Quantity)
            };

            // Initialize all months to 0
            foreach (var month in MonthHeaders)
                row.MonthlySales[month] = 0;

            // Group sales by month and sum quantities
            var salesByMonth = sales
                .Where(s => s.PartId == part.Id)
                .GroupBy(s => s.SaleDate.ToString("yy-MM"))
                .ToDictionary(g => g.Key, g => g.Sum(s => s.Quantity));

            foreach (var month in MonthHeaders)
            {
                if (salesByMonth.TryGetValue(month, out int qty))
                    row.MonthlySales[month] = qty;
            }

            SalesRows.Add(row);
        }
    }
}

public class SalesHistoryRow
{
    public string PartId { get; set; }
    public string Description { get; set; }
    public int TotalSales { get; set; }
    // Key: "yy MM", Value: sales for that month
    public Dictionary<string, int> MonthlySales { get; set; } = new();
}