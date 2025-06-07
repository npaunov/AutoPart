
namespace AutoPartApp.DTO.Orders;

/// <summary>
/// Represents a store with a unique key and a localized display name.
/// </summary>
public class StoreDto
{
    /// <summary>
    /// Gets or sets the unique key for the store (e.g., "Sofia", "Plovdiv").
    /// </summary>
    public string Key { get; set; }

    /// <summary>
    /// Gets or sets the localized display name for the store.
    /// </summary>
    public string DisplayName { get; set; }
}
