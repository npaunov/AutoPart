using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using AutoPartApp.Utilities;
using AutoPartApp.EntityFramework;
using Microsoft.EntityFrameworkCore;
using AutoPartApp.Models;

namespace AutoPartApp;

/// <summary>
/// ViewModel for the Data Import functionality.
/// </summary>
public partial class DataImportViewModel : ObservableObject
{
    [ObservableProperty]
    private string _selectedFilePath = string.Empty;
    [ObservableProperty]
    private string _buttonStatus = string.Empty;

    // Expose WarehouseViewModel as a property
    public WarehouseViewModel WarehouseViewModel { get; }

    private readonly IDialogService _dialogService;
    private readonly AutoPartDbContext _context;

    public DataImportViewModel(
        WarehouseViewModel warehouseViewModel,
        IDialogService dialogService,
        AutoPartDbContext context)
    {
        WarehouseViewModel = warehouseViewModel;
        _dialogService = dialogService;
        _context = context;
    }

    [RelayCommand]
    /// <summary>
    /// Asks for confirmation and creates a new database if confirmed, then updates the status message.
    /// </summary>
    private void CreateDatabase()
    {
        if (_dialogService.ShowConfirmation(
            Properties.Strings.CreateDatabaseQuestionName,
            Properties.Strings.ConfirmCreateDatabaseName
        ))
        {
            try
            {
                _context.Database.EnsureDeleted(); // Optional: deletes the existing database
                _context.Database.Migrate();       // Applies all migrations (creates schema)
                ButtonStatus = "Database created successfully!";
            }
            catch (Exception ex)
            {
                ButtonStatus = $"Failed to create database: {ex.Message}";
            }
        }
        else
        {
            ButtonStatus = Properties.Strings.CreateDatabaseCanceledName;
        }
    }

    [RelayCommand]
    private void PopulateDatabase()
    {
        if (!_dialogService.ShowConfirmation(
            "Populate data from CSV?",
            "Confirm Populate Data"))
        {
            ButtonStatus = "Populate database canceled.";
            return;
        }

        // Call the BrowseAndImport method to select and import the CSV
        BrowseAndImport();

        // Check if import was successful
        if (DataImportUtil.ImportedParts == null || DataImportUtil.ImportedParts.Count == 0)
        {
            ButtonStatus = "No imported parts to add. Please import a valid CSV file.";
            return;
        }
        DeleteAllData();
        int added = PopulatePartsTable();
        PopulateSalesTotalTable();
        PopulateSalesTable();
        _context.SaveChanges();
        ButtonStatus = $"Database populated from CSV. {added} new parts added.";
        WarehouseViewModel.LoadImportedParts();
    }


    [RelayCommand]
    /// <summary>
    /// Opens a file dialog to select a CSV file and imports data from it.
    /// </summary>
    private void BrowseAndImport()
    {
        var openFileDialog = new Microsoft.Win32.OpenFileDialog
        {
            Filter = "CSV Files (*.csv)|*.csv|All Files (*.*)|*.*"
        };

        if (openFileDialog.ShowDialog() == true)
        {
            SelectedFilePath = openFileDialog.FileName;
            ButtonStatus = DataImportUtil.ImportCsv(SelectedFilePath);
            WarehouseViewModel.LoadImportedParts();
        }
    }

    [RelayCommand]
    private void DeleteCsvData()
    {
        if (_dialogService.ShowConfirmation(
            Properties.Strings.DeleteCSVDataQuestionName,
            Properties.Strings.ConfirmDeleteName))
        {
            DataImportUtil.ImportedParts.Clear();
        }

    }

    [RelayCommand]
    private void TestAction()
    {
        ButtonStatus = "Test button clicked!";
        //try
        //{

        //    // Get the current directory
        //    string currentDirectory = Directory.GetCurrentDirectory();
        //    string relativePath = Path.Combine(currentDirectory, @"..\..\..\..\DataBase\empty_file.txt");

        //    File.Create(relativePath);
        //    ButtonStatus = $"Empty .txt file created";
        //}
        //catch (Exception ex)
        //{
        //    ButtonStatus = $"Failed to create .txt file: {ex.Message}";
        //}
    }
    private int PopulatePartsTable()
    {
        int added = 0;
        foreach (var part in DataImportUtil.ImportedParts)
        {
            if (!_context.PartsInStock.Any(p => p.Id == part.Id))
            {
                _context.PartsInStock.Add(part);
                added++;
            }
        }
        return added;
    }

    private void PopulateSalesTotalTable()
    {
        foreach (var part in DataImportUtil.ImportedParts)
        {
            var totalSales = part.InStore * 18;
            var salesTotal = new PartsSalesTotal
            {
                Id = part.Id,
                TotalSales = totalSales
            };
            _context.PartsSalesTotals.Add(salesTotal);
        }
    }

    private void PopulateSalesTable()
    {
        var random = new Random();
        DateTime start = new DateTime(2022, 7, 1);
        DateTime end = new DateTime(2025, 6, 30);
        int monthsCount = ((end.Year - start.Year) * 12) + end.Month - start.Month + 1;

        var allSales = new List<PartSale>();

        foreach (var part in DataImportUtil.ImportedParts)
        {
            int totalSales = part.InStore * 18;
            if (totalSales <= 0)
                continue;

            var months = Enumerable.Range(0, monthsCount)
                .Select(i => start.AddMonths(i))
                .ToList();

            int monthsWithSalesCount = random.Next((int)(monthsCount * 0.7), (int)(monthsCount * 0.8) + 1);
            var monthsWithSales = months.OrderBy(_ => random.Next()).Take(monthsWithSalesCount).OrderBy(m => m).ToList();

            double avgSales = (double)totalSales / monthsWithSalesCount;
            var salesPerMonth = new List<int>();
            int salesLeft = totalSales;
            for (int i = 0; i < monthsWithSalesCount; i++)
            {
                int min = (int)Math.Floor(avgSales * 0.7);
                int max = (int)Math.Ceiling(avgSales * 1.3);

                int sales;
                if (i == monthsWithSalesCount - 1)
                {
                    sales = salesLeft;
                }
                else
                {
                    int maxAllowed = salesLeft - (min * (monthsWithSalesCount - i - 1));
                    max = Math.Min(max, maxAllowed);
                    min = Math.Max(min, 0);
                    if (max < min) max = min;
                    sales = random.Next(min, max + 1);
                }
                salesPerMonth.Add(sales);
                salesLeft -= sales;
            }

            for (int i = 0; i < monthsWithSalesCount; i++)
            {
                var month = monthsWithSales[i];
                int daysInMonth = DateTime.DaysInMonth(month.Year, month.Month);
                int day = random.Next(1, daysInMonth + 1);
                var saleDate = new DateTime(month.Year, month.Month, day);

                var sale = new PartSale
                {
                    PartId = part.Id,
                    SaleDate = saleDate,
                    Quantity = salesPerMonth[i]
                };
                allSales.Add(sale);
            }
        }

        // Sort all sales by SaleDate before inserting
        foreach (var sale in allSales.OrderBy(s => s.SaleDate))
        {
            _context.PartSales.Add(sale);
        }
    }

    public void DeleteAllData()
    {
        _context.PartSales.RemoveRange(_context.PartSales);
        _context.PartsSalesTotals.RemoveRange(_context.PartsSalesTotals);
        _context.PartsInStock.RemoveRange(_context.PartsInStock);
        // _context.Orders.RemoveRange(_context.Orders);
        // _context.OrderItems.RemoveRange(_context.OrderItems);
        _context.SaveChanges();
    }

    // Localized string properties
    public string DataImportName => Properties.Strings.DataImportName;
    public string SelectFileName => Properties.Strings.SelectFileName;
    public string ImportCSVName => Properties.Strings.ImportCSVName;
    public string CreateNewDataBaseName => Properties.Strings.CreateNewDataBaseName;
    public string DataAddToDBName => Properties.Strings.DataAddToDBName;
    public string ModifyDataName => Properties.Strings.ModifyDataName;
    public string DeleteCSVDataName => Properties.Strings.DeleteCSVDataName;

}
