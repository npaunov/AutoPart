using AutoPartApp.Services;
using System.IO;
using System.Windows.Input;

namespace AutoPartApp;

/// <summary>
/// ViewModel for the Data Import functionality.
/// </summary>
public class DataImportViewModel : BaseViewModel
{
    private string _selectedFilePath = string.Empty;
    private string _buttonStatus = string.Empty;

    // Expose WarehouseViewModel as a property
    public WarehouseViewModel WarehouseViewModel { get; } = new();

    #region Properties
    /// <summary>
    /// The path of the selected file.
    /// </summary>
    public string SelectedFilePath
    {
        get => _selectedFilePath;
        set
        {
            _selectedFilePath = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// The status message of the button operation.
    /// </summary>
    public string ButtonStatus
    {
        get => _buttonStatus;
        set
        {
            _buttonStatus = value;
            OnPropertyChanged();
        }
    }
    #endregion Properties

    /// <summary>
    /// Command to browse and import data from a selected file.
    /// </summary>
    public ICommand BrowseAndImportCommand { get; }
    public ICommand TestCommand { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="DataImportViewModel"/> class.
    /// </summary>
    public DataImportViewModel()
    {
        BrowseAndImportCommand = new RelayCommand(BrowseAndImport);
        TestCommand = new RelayCommand(TestAction);
    }



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
}
