namespace SipPOS.DataTransfer.General;

/// <summary>
/// Data transfer object for filtering special offers.
/// </summary>
public class SpecialOfferFilterDto
{
    /// <summary>
    /// Gets or sets the name of the special offer.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the code of the special offer.
    /// </summary>
    public string? Code { get; set; }

    /// <summary>
    /// Gets or sets the description of the special offer.
    /// </summary>
    public string? Desc { get; set; }

    /// <summary>
    /// Gets or sets the category ID of the special offer.
    /// </summary>
    public long? CategoryId { get; set; }

    /// <summary>
    /// Gets or sets the product ID of the special offer.
    /// </summary>
    public long? ProductId { get; set; }

    /// <summary>
    /// Gets or sets the status of the special offer.
    /// </summary>
    public string? Status { get; set; }
}
