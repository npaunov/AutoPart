using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Services.DIServices.Interfaces;
using AutoPartApp.Managers;
using AutoPartApp.EntityFramework;

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

    public DataImportViewModel(WarehouseViewModel warehouseViewModel, IDialogService dialogService)
    {
        WarehouseViewModel = warehouseViewModel;
        _dialogService = dialogService;
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
            ButtonStatus = DbContextWrapper.CreateNewDatabase();
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
            ButtonStatus = DataImportManager.ImportCsv(SelectedFilePath);
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
            DataImportManager.ImportedParts.Clear();
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
