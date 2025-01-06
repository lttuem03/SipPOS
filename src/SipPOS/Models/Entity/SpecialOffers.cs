using System.ComponentModel;

namespace SipPOS.Models.Entity;

public class SpecialOffer : BaseEntity, INotifyPropertyChanged
{
    public long StoreId { get; set; }

    public string Code { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public decimal ItemsSold { get; set; } = 0m;

    public decimal MaxItems { get; set; } = 0m;

    public string? Type { get; set; } = string.Empty;

    public string? PriceType { get; set; } = string.Empty;

    public long? CategoryId { get; set; }

    public long? ProductId { get; set; }

    public DateTime? StartDate { get; set; } = DateTime.MinValue;

    public DateTime? EndDate { get; set; } = DateTime.MinValue;

    public decimal? DiscountPrice { get; set; } = 0m;

    public decimal? DiscountPercentage { get; set; } = 0.0m;

    public string Status { get; set; } = string.Empty;

    public event PropertyChangedEventHandler? PropertyChanged;
}