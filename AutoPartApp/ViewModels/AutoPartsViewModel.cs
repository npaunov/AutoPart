using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Models;
using System.Collections.ObjectModel;

namespace AutoPartApp;

public partial class AutoPartsViewModel : ObservableObject
{
    [ObservableProperty]
    private Part? _selectedPart;

    public ObservableCollection<Part> Parts { get; set; } = new();

    [RelayCommand(CanExecute = nameof(CanRemovePart))]
    private void RemovePart()
    {
        if (SelectedPart != null)
        {
            Parts.Remove(SelectedPart);
            SelectedPart = null;
        }
    }

    [RelayCommand]
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

    private bool CanRemovePart() => SelectedPart != null;
}