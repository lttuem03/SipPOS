namespace SipPOS.Models.Entity;

/// <summary>
/// Represents a category entity.
/// </summary>
public class Category : BaseEntity
{
    /// <summary>
    /// Gets or sets the name of the category.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the description of the category.
    /// </summary>
    public string? Desc { get; set; }

    /// <summary>
    /// Gets or sets the status of the category.
    /// </summary>
    public string? Status { get; set; }

    /// <summary>
    /// Gets or sets the list of image URLs associated with the category.
    /// </summary>
    public IList<string> ImageUrls { get; set; } = new List<string>();
}
