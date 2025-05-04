using System.Collections.Generic;

namespace AutoPartApp;

public class Warehouse
{
    public Dictionary<int, int> PartQuantities { get; set; } = new();
}
