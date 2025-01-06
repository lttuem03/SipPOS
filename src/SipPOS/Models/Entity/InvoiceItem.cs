using System.ComponentModel;

using SipPOS.DataTransfer.Entity;

namespace SipPOS.Models.Entity;

/// <summary>
/// Represents an invoice item model.
/// </summary>
public class InvoiceItem : INotifyPropertyChanged
{
    /// <summary>
    /// Gets the ID of the invoice item.
    /// </summary>
    public long Id { get; }

    /// <summary>
    /// Gets the invoice ID to which this item belongs.
    /// </summary>
    public long InvoiceId { get; }

    /// <summary>
    /// Gets the product ID of the invoice item.
    /// </summary>
    public long ProductId { get; }

    /// <summary>
    /// Gets the ordinal position of the invoice item.
    /// </summary>
    public long Ordinal { get; }

    /// <summary>
    /// Gets the name of the item.
    /// </summary>
    public string ItemName { get; }

    /// <summary>
    /// Gets the name of the option.
    /// </summary>
    public string OptionName { get; }

    /// <summary>
    /// Gets the price of the option.
    /// </summary>
    public decimal OptionPrice { get; }

    /// <summary>
    /// Gets the discount applied to the item.
    /// </summary>
    public decimal Discount { get; }

    /// <summary>
    /// Gets the note associated with the item.
    /// </summary>
    public string Note { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="InvoiceItem"/> class.
    /// </summary>
    /// <param name="dto">The data transfer object to initialize from.</param>
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

    /// <summary>
    /// Occurs when a property value changes.
    /// </summary>
    public event PropertyChangedEventHandler? PropertyChanged;

    // This event is for TrulyObservableCollection because 
    // ObservableCollection is on Microsoft's github issue
    // and can't be bind with a ListView
}
