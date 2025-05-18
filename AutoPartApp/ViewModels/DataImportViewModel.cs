using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Input;
using EntityFramework;
using CommunityToolkit.Mvvm.Input;

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
     

    /// <summary>
    /// Initializes a new instance of the <see cref="DataImportViewModel"/> class.
    /// </summary>
    public DataImportViewModel(WarehouseViewModel warehouseViewModel)
    {
        WarehouseViewModel = warehouseViewModel;
    }

    [RelayCommand]
    /// <summary>
    /// Creates a new database and updates the status message.
    /// </summary>
    private void CreateDatabase()
    {
        ButtonStatus = DbContextWrapper.CreateNewDatabase();
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
            ButtonStatus = DataImportService.ImportCsv(SelectedFilePath);
            WarehouseViewModel.LoadImportedParts();
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

}
