namespace SipPOS.DataTransfer.General;

/// <summary>
/// Data transfer object for filtering categories.
/// </summary>
public class CategoryFilterDto
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
}
