namespace SipPOS.Models.General;

/// <summary>
/// Represents a paginated list of items.
/// </summary>
/// <typeparam name="T">The type of items in the paginated list.</typeparam>
public class Pagination<T>
{
    /// <summary>
    /// Gets or sets the current page number.
    /// </summary>
    public int Page { get; set; } = 1;

    /// <summary>
    /// Gets or sets the total number of pages.
    /// </summary>
    public int TotalPage { get; set; } = 1;

    /// <summary>
    /// Gets or sets the number of items per page.
    /// </summary>
    public int PerPage { get; set; } = 5;

    /// <summary>
    /// Gets or sets the total number of records.
    /// </summary>
    public long TotalRecord { get; set; } = 0;

    /// <summary>
    /// Gets or sets the list of items for the current page.
    /// </summary>
    public IList<T> Data { get; set; } = new List<T>();
}
