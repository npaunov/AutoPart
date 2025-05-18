using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AutoPartApp.Models;

public class Warehouse
{
    public ObservableCollection<Part> Parts { get; set; } = new();
}