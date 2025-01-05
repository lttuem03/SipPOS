namespace SipPOS.Models.Entity;

using System.ComponentModel;
using SipPOS.DataTransfer.Entity;

public class InvoiceItem : INotifyPropertyChanged
{
    public long Id { get; }

    public long InvoiceId { get; }

    public long ProductId { get; }

    public long Ordinal { get; }

    public string ItemName { get; }

    public string OptionName { get; }

    public decimal OptionPrice { get; }

    public decimal Discount { get; }

    public string Note { get; }

    public InvoiceItem(InvoiceItemDto dto)
    {
        Id = dto.Id;
        InvoiceId = dto.InvoiceId;
        ProductId = dto.ProductId;
        Ordinal = dto.Ordinal;
        ItemName = dto.ItemName;
        OptionName = dto.OptionName;
        OptionPrice = dto.OptionPrice;
        Discount = dto.Discount;
        Note = dto.Note;
    }

    public event PropertyChangedEventHandler? PropertyChanged;
}