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
        /// <param name="currentDate">The reference date for calculations (default is DateTime.Now).</param>
        /// <returns>A list of <see cref="OrderSuggestionDto"/> representing suggested orders.</returns>
        public static List<OrderSuggestionDto> GetOrderSuggestions(
            List<Part> parts,
            List<PartSale> sales,
            int monthsToOrder = 2,
            int salesHistoryMonths = 36,
            DateTime? currentDate = null)
        {
            var referenceDate = currentDate ?? DateTime.Now;
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
                    var salesHistory = GetSalesForColumns(sales.Where(s => s.PartId == part.Id).ToList(), referenceDate);

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
                        Sales5 = salesHistory[4]
                    });
                }
            }

            return suggestions;
        }

        /// <summary>
        /// Gets the sales quantities for the last month, also curent and next month 1 and 2 years ago.
        /// </summary>
        /// <param name="sales">List of sales for the part.</param>
        /// <param name="dateTimeNow">The current date (reference point).</param>
        /// <returns>
        /// Array of Five sales quantities in the order:
        /// [0] = last month
        /// [1] = 1 year ago, current month
        /// [2] = 1 years ago, next month
        /// [3] = 2 years ago, current month
        /// [4] = 2 years ago, next month
        /// </returns>
        public static int[] GetSalesForColumns(List<PartSale> sales, DateTime dateTimeNow)
        {
            int[] result = new int[5];

            // Sales 1: Last month
            var lastMonth = dateTimeNow.AddMonths(-1);
            result[0] = sales.Where(s => s.SaleDate.Year == lastMonth.Year && s.SaleDate.Month == lastMonth.Month)
                             .Sum(s => s.Quantity);

            // Sales 2: Same month last year
            var sameMonthLastYear = dateTimeNow.AddYears(-1);
            result[1] = sales.Where(s => s.SaleDate.Year == sameMonthLastYear.Year && s.SaleDate.Month == sameMonthLastYear.Month)
                             .Sum(s => s.Quantity);

            // Sales 3: Next month last year
            var nextMonthLastYear = dateTimeNow.AddYears(-1).AddMonths(1);
            result[2] = sales.Where(s => s.SaleDate.Year == nextMonthLastYear.Year && s.SaleDate.Month == nextMonthLastYear.Month)
                             .Sum(s => s.Quantity);

            // Sales 4: Same month two years ago
            var sameMonthTwoYearsAgo = dateTimeNow.AddYears(-2);
            result[3] = sales.Where(s => s.SaleDate.Year == sameMonthTwoYearsAgo.Year && s.SaleDate.Month == sameMonthTwoYearsAgo.Month)
                             .Sum(s => s.Quantity);

            // Sales 5: Next month two years ago
            var nextMonthTwoYearsAgo = dateTimeNow.AddYears(-2).AddMonths(1);
            result[4] = sales.Where(s => s.SaleDate.Year == nextMonthTwoYearsAgo.Year && s.SaleDate.Month == nextMonthTwoYearsAgo.Month)
                             .Sum(s => s.Quantity);

            return result;
        }
    }
}
