namespace SipPOS.DataTransfer.General;

/// <summary>
/// Data transfer object for filtering products.
/// </summary>
public class SpecialOfferFilterDto
{
    /// <summary>
    /// Gets or sets the name of the product.
    /// </summary>
    public string? Name { get; set; }

    public string? Code { get; set; }

    /// <summary>
    /// Gets or sets the description of the product.
    /// </summary>
    public string? Desc { get; set; }

    /// <summary>
    /// Gets or sets the category ID of the product.
    /// </summary>
    public long? CategoryId { get; set; }

    public long? ProductId { get; set; }

    /// <summary>
    /// Gets or sets the status of the product.
    /// </summary>
    public string? Status { get; set; }
}
