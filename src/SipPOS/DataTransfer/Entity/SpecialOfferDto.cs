namespace SipPOS.DataTransfer.Entity;

public class SpecialOfferDto : BaseEntityDto
{
    public long StoreId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public long ItemsSold { get; set; } = 0;

    public long? CategoryId { get; set; }

    public string Status { get; set; } = string.Empty;

    public bool IsSelected { get; set; } = false;
}