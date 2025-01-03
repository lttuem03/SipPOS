using System.ComponentModel;

namespace SipPOS.Models.Entity;

public class SpecialOffer : BaseEntity
{
    public long StoreId { get; set; }

    public string Code { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public double SoldItems { get; set; } = 0;

    public double MaxItems { get; set; } = 0;

    public string? Type { get; set; }

    public string? PriceType { get; set; }

    public long? CategoryId { get; set; }

    public long? ProductId { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public double? DiscountPrice { get; set; }

    public double? DiscountPercentage { get; set; }

    public string Status { get; set; } = string.Empty;

}