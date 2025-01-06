namespace SipPOS.DataTransfer.Entity;

/// <summary>
/// Represents a product option data transfer object.
/// </summary>
public class ProductOptionDto
{
    /// <summary>
    /// Gets or sets the ID of the product option.
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the option name.
    /// </summary>
    public string? Option { get; set; }

    /// <summary>
    /// Gets or sets the price of the option.
    /// </summary>
    public decimal? Price { get; set; }
}
