using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace AutoPartApp;

public class AutoPartsViewModel : BaseViewModel
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
        Parts.Add(new Part { Id = "1", Description = "Brake Pad", PriceBGN = 29.99m });
        Parts.Add(new Part { Id = "2", Description = "Oil Filter", PriceBGN = 15.49m });

        AddPartCommand = new RelayCommand(AddPart);
        RemovePartCommand = new RelayCommand(RemovePart, CanRemovePart);
    }

    private void AddPart()
    {
        var newPart = new Part
        {
            Id = (Parts.Count + 1).ToString(),
            Description = "New Part",
            PriceBGN = 0.0m
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
}
