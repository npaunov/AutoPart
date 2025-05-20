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

    // Localized string properties
    public string DataImportName => Properties.Strings.DataImportName;
    public string SelectFileName => Properties.Strings.SelectFileName;
    public string ImportCSVName => Properties.Strings.ImportCSVName;
    public string CreateNewDataBaseName => Properties.Strings.CreateNewDataBaseName;
    public string ModifyDataName => Properties.Strings.ModifyDataName;
    public string DeleteCSVDataName => Properties.Strings.DeleteCSVDataName;

}
