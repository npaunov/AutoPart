using System.Collections.ObjectModel;

namespace AutoPart.Models;

public class Warehouse
{
    public ObservableCollection<Part> PartsInStock { get; set; } = new();
}