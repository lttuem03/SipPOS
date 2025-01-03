namespace SipPOS.DataTransfer.Entity;

/// <summary>
/// Data transfer object for Category.
/// </summary>
public class CategoryDto : BaseEntityDto
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
    public string? Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a value indicating whether the category is selected.
    /// </summary>
    public bool IsSelected { get; set; } = false;
}
