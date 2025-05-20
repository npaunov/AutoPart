using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using AutoPartApp.Utilities;
using AutoPartApp.EntityFramework;
using Microsoft.EntityFrameworkCore;

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
            if (!_context.Parts.Any(p => p.Id == part.Id))
            {
                _context.Parts.Add(part);
                added++;
            }
        }
        return added;
    }

    public void DeleteAllData()
    {
        _context.Parts.RemoveRange(_context.Parts);
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
