using System;
using System.Collections.Generic;
using System.Linq;
using AutoPartApp.Models;

namespace AutoPartApp
{
    /// <summary>
    /// Provides business logic for generating order suggestions based on sales history and inventory.
    /// </summary>
    public static class OrdersLogic
    {
        /// <summary>
        /// Generates a list of order suggestions for parts that are below the required stock level.
        /// </summary>
        /// <param name="parts">The list of parts currently in stock.</param>
        /// <param name="sales">The list of all part sales.</param>
        /// <param name="monthsToOrder">The number of months to cover in the order (default is 2).</param>
        /// <param name="salesHistoryMonths">The number of months to use for average sales calculation (default is 36).</param>
        /// <param name="now">The reference date for calculations (default is DateTime.Now).</param>
        /// <returns>A list of <see cref="OrderSuggestionDto"/> representing suggested orders.</returns>
        public static List<OrderSuggestionDto> GetOrderSuggestions(
            List<Part> parts,
            List<PartSale> sales,
            int monthsToOrder = 2,
            int salesHistoryMonths = 36,
            DateTime? now = null)
        {
            var referenceDate = now ?? DateTime.Now;
            var suggestions = new List<OrderSuggestionDto>();

            foreach (var part in parts)
            {
                // Filter sales for this part and for the last N months
                var partSales = sales
                    .Where(s => s.PartId == part.Id && s.SaleDate >= referenceDate.AddMonths(-salesHistoryMonths))
                    .ToList();

                int totalSales = partSales.Sum(s => s.Quantity);
                double avgMonthlySales = salesHistoryMonths > 0 ? (double)totalSales / salesHistoryMonths : 0;
                int neededQty = (int)Math.Ceiling(avgMonthlySales * monthsToOrder);

                // Only suggest if current stock is less than needed
                if (part.InStore < neededQty)
                {
                    // Get last 6 sales for the part (1, 2, 3 years ago, current and next month)
                    var salesHistory = GetLastSixSales(sales.Where(s => s.PartId == part.Id).ToList(), referenceDate);

                    suggestions.Add(new OrderSuggestionDto
                    {
                        PartId = part.Id,
                        Description = part.Description,
                        Package = part.Package,
                        NeededQty = Math.Max(neededQty - part.InStore, 0),
                        PriceBGN = part.PriceBGN,
                        PriceEURO = part.PriceEURO,
                        Sales1 = salesHistory[0],
                        Sales2 = salesHistory[1],
                        Sales3 = salesHistory[2],
                        Sales4 = salesHistory[3],
                        Sales5 = salesHistory[4],
                        Sales6 = salesHistory[5]
                    });
                }
            }

            return suggestions;
        }

        /// <summary>
        /// Gets the sales quantities for the current and next month, 1, 2, and 3 years ago.
        /// </summary>
        /// <param name="sales">List of sales for the part.</param>
        /// <param name="now">The current date (reference point).</param>
        /// <returns>
        /// Array of Six sales quantities in the order:
        /// [0] = 1 year ago, current month
        /// [1] = 1 year ago, next month
        /// [2] = 2 years ago, current month
        /// [3] = 2 years ago, next month
        /// [4] = 3 years ago, current month
        /// [5] = 3 years ago, next month
        /// </returns>
        public static int[] GetLastSixSales(List<PartSale> sales, DateTime now)
        {
            int[] result = new int[6];
            for (int year = 1; year <= 3; year++)
            {
                // Current month, N years ago
                var monthCurrent = new DateTime(now.Year - year, now.Month, 1);
                result[(year - 1) * 2] = sales
                    .Where(s => s.SaleDate.Year == monthCurrent.Year && s.SaleDate.Month == monthCurrent.Month)
                    .Sum(s => s.Quantity);

                // Next month, N years ago
                var monthNext = monthCurrent.AddMonths(1);
                result[(year - 1) * 2 + 1] = sales
                    .Where(s => s.SaleDate.Year == monthNext.Year && s.SaleDate.Month == monthNext.Month)
                    .Sum(s => s.Quantity);
            }
            return result;
        }
    }
}
