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

    // Expose WarehouseViewModel as a property
    public WarehouseViewModel WarehouseViewModel { get; } = new();

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
    /// Command to browse and import data from a selected file.
    /// </summary>
    public ICommand BrowseAndImportCommand { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="DataImportViewModel"/> class.
    /// </summary>
    public DataImportViewModel()
    {
        BrowseAndImportCommand = new RelayCommand(BrowseAndImport);
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
            ImportStatus = DataImportService.ImportCsv(SelectedFilePath);
            WarehouseViewModel.LoadImportedParts();
        }
    }
}
