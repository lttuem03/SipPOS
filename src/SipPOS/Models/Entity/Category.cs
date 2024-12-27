namespace SipPOS.Models.Entity;

/// <summary>
/// Represents a category entity.
/// </summary>
public class Category : BaseEntity
{
    /// <summary>
    /// Gets or sets the store id of the category.
    /// </summary>
    public long StoreId { get; set; }

    /// <summary>
    /// Gets or sets the name of the category.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the description of the category.
    /// </summary>
    public string? Description { get; set; }
}
