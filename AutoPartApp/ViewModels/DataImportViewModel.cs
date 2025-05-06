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
    private string _importStatus = string.Empty;
    private WarehouseViewModel _warehouseViewModel = new();
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
    /// The status message of the import operation.
    /// </summary>
    public string ImportStatus
    {
        get => _importStatus;
        set
        {
            _importStatus = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// Command to browse and select a file.
    /// </summary>
    public ICommand BrowseFileCommand { get; }

    /// <summary>
    /// Command to import data from the selected file.
    /// </summary>
    public ICommand ImportDataCommand { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="DataImportViewModel"/> class.
    /// </summary>
    public DataImportViewModel()
    {
        BrowseFileCommand = new RelayCommand(BrowseFile);
        ImportDataCommand = new RelayCommand(ImportData);
    }

    /// <summary>
    /// Opens a file dialog to select a CSV file.
    /// </summary>
    private void BrowseFile()
    {
        var openFileDialog = new Microsoft.Win32.OpenFileDialog
        {
            Filter = "CSV Files (*.csv)|*.csv|All Files (*.*)|*.*"
        };

        if (openFileDialog.ShowDialog() == true)
        {
            SelectedFilePath = openFileDialog.FileName;
        }
    }

    /// <summary>
    /// Imports data from the selected file using the <see cref="DataImportService"/>.
    /// </summary>
    private void ImportData()
    {
        ImportStatus = DataImportService.ImportCsv(SelectedFilePath);
        _warehouseViewModel.LoadImportedParts();
    }
}
