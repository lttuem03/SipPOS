using System.ComponentModel;

namespace SipPOS.Models.Entity;

/// <summary>
/// Represents a special offer entity.
/// </summary>
public class SpecialOffer : BaseEntity, INotifyPropertyChanged
{
    /// <summary>
    /// Gets or sets the store ID.
    /// </summary>
    public long StoreId { get; set; }

    /// <summary>
    /// Gets or sets the code of the special offer.
    /// </summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the name of the special offer.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the description of the special offer.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the number of items sold under the special offer.
    /// </summary>
    public decimal ItemsSold { get; set; } = 0m;

    /// <summary>
    /// Gets or sets the maximum number of items allowed under the special offer.
    /// </summary>
    public decimal MaxItems { get; set; } = 0m;

    /// <summary>
    /// Gets or sets the type of the special offer.
    /// </summary>
    public string? Type { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the price type of the special offer.
    /// </summary>
    public string? PriceType { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the category ID associated with the special offer.
    /// </summary>
    public long? CategoryId { get; set; }

    /// <summary>
    /// Gets or sets the product ID associated with the special offer.
    /// </summary>
    public long? ProductId { get; set; }

    /// <summary>
    /// Gets or sets the start date of the special offer.
    /// </summary>
    public DateTime? StartDate { get; set; } = DateTime.MinValue;

    /// <summary>
    /// Gets or sets the end date of the special offer.
    /// </summary>
    public DateTime? EndDate { get; set; } = DateTime.MinValue;

    /// <summary>
    /// Gets or sets the discount price of the special offer.
    /// </summary>
    public decimal? DiscountPrice { get; set; } = 0m;

    /// <summary>
    /// Gets or sets the discount percentage of the special offer.
    /// </summary>
    public decimal? DiscountPercentage { get; set; } = 0.0m;

    /// <summary>
    /// Gets or sets the status of the special offer.
    /// </summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// Occurs when a property value changes.
    /// </summary>
    public event PropertyChangedEventHandler? PropertyChanged;
}
