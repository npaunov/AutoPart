using System.IO;
using System.Windows.Input;

namespace AutoPartApp
{
    public class DataImportViewModel : BaseViewModel
    {
        private string _selectedFilePath = string.Empty;
        private string _importStatus = string.Empty;

        public string SelectedFilePath
        {
            get => _selectedFilePath;
            set
            {
                _selectedFilePath = value;
                OnPropertyChanged();
            }
        }

        public string ImportStatus
        {
            get => _importStatus;
            set
            {
                _importStatus = value;
                OnPropertyChanged();
            }
        }

        public ICommand BrowseFileCommand { get; }
        public ICommand ImportDataCommand { get; }

        public DataImportViewModel()
        {
            BrowseFileCommand = new RelayCommand(BrowseFile);
            ImportDataCommand = new RelayCommand(ImportData);
        }

        private void BrowseFile()
        {
            // Open a file dialog to select a file
            var openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "CSV Files (*.csv)|*.csv|All Files (*.*)|*.*"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                SelectedFilePath = openFileDialog.FileName;
            }
        }

        private void ImportData()
        {
            if (string.IsNullOrEmpty(SelectedFilePath) || !File.Exists(SelectedFilePath))
            {
                ImportStatus = "Invalid file path. Please select a valid file.";
                return;
            }

            try
            {
                // Simulate data import logic
                var fileContent = File.ReadAllText(SelectedFilePath);
                // Process the file content (e.g., parse CSV and update the database)
                ImportStatus = "Data imported successfully!";
            }
            catch
            {
                ImportStatus = "An error occurred during data import.";
            }
        }
    }
}
