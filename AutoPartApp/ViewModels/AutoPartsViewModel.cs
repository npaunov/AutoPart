using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace AutoPartApp.ViewModels;

public class AutoPartsViewModel : INotifyPropertyChanged
{
    private Part? _selectedPart;

    public ObservableCollection<Part> Parts { get; set; } = new();

    public Part? SelectedPart
    {
        get => _selectedPart;
        set
        {
            _selectedPart = value;
            OnPropertyChanged();
        }
    }

    public ICommand AddPartCommand { get; }
    public ICommand RemovePartCommand { get; }

    public AutoPartsViewModel()
    {
        // Initialize sample data
        Parts.Add(new Part { Id = 1, Description = "Brake Pad", Price = 29.99m, SalesForYear = 500 });
        Parts.Add(new Part { Id = 2, Description = "Oil Filter", Price = 15.49m, SalesForYear = 1200 });

        AddPartCommand = new RelayCommand(AddPart);
        RemovePartCommand = new RelayCommand(RemovePart, CanRemovePart);
    }

    private void AddPart()
    {
        var newPart = new Part
        {
            Id = Parts.Count + 1,
            Description = "New Part",
            Price = 0.0m,
            SalesForYear = 0
        };
        Parts.Add(newPart);
    }

    private void RemovePart()
    {
        if (SelectedPart != null)
        {
            Parts.Remove(SelectedPart);
            SelectedPart = null;
        }
    }

    private bool CanRemovePart() => SelectedPart != null;

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
