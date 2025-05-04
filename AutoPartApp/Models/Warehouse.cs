using System.Collections.Generic;

namespace AutoPartApp.Models;

public class Warehouse
{
    public Dictionary<int, int> PartQuantities { get; set; } = new();
}
