namespace SipPOS.DataTransfer.General;

/// <summary>
/// Data transfer object for sorting.
/// </summary>
public class SortDto
{
    /// <summary>
    /// Gets or sets the field to sort by.
    /// </summary>
    public string? SortBy { get; set; } = "created_at";

    /// <summary>
    /// Gets or sets the sort type (e.g., Asc or Desc).
    /// </summary>
    public string? SortType { get; set; } = "Asc";
}
