using System.ComponentModel;

using SipPOS.DataTransfer.Entity;

namespace SipPOS.Models.Entity;

/// <summary>
/// Represents an invoice model.
/// </summary>
public class Invoice : INotifyPropertyChanged
{
    /// <summary>
    /// Gets the invoice ID.
    /// </summary>
    public long Id { get; }

    /// <summary>
    /// Gets the staff ID.
    /// </summary>
    public long StaffId { get; }

    /// <summary>
    /// Gets the staff name.
    /// </summary>
    public string StaffName { get; }

    /// <summary>
    /// Gets the creation date and time of the invoice.
    /// </summary>
    public DateTime CreatedAt { get; }

    /// <summary>
    /// Gets the list of invoice items.
    /// </summary>
    public List<InvoiceItem> InvoiceItems { get; }

    /// <summary>
    /// Gets the total number of items in the invoice.
    /// </summary>
    public long ItemCount { get; }

    /// <summary>
    /// Gets the subtotal amount of the invoice.
    /// </summary>
    public decimal SubTotal { get; }

    /// <summary>
    /// Gets the total discount amount of the invoice.
    /// </summary>
    public decimal TotalDiscount { get; }

    /// <summary>
    /// Gets the VAT amount based on the invoice.
    /// </summary>
    public decimal InvoiceBasedVAT { get; }

    /// <summary>
    /// Gets the total amount of the invoice.
    /// </summary>
    public decimal Total { get; }

    /// <summary>
    /// Gets the amount paid by the customer.
    /// </summary>
    public decimal Paid { get; }

    /// <summary>
    /// Gets the change amount to be returned to the customer.
    /// </summary>
    public decimal Change { get; }

    /// <summary>
    /// Gets the coupon code applied to the invoice.
    /// </summary>
    public string CouponCode { get; }

    /// <summary>
    /// Gets the payment method used for the invoice.
    /// </summary>
    public string PaymentMethod { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Invoice"/> class.
    /// </summary>
    /// <param name="dto">The data transfer object to initialize from.</param>
    public Invoice(InvoiceDto dto)
    {
        Id = dto.Id;
        StaffId = dto.StaffId;
        StaffName = dto.StaffName;
        CreatedAt = dto.CreatedAt;

        InvoiceItems = new();
        foreach (var invoiceItemDto in dto.InvoiceItems)
        {
            InvoiceItems.Add(new InvoiceItem(invoiceItemDto));
        }

        ItemCount = dto.ItemCount;
        SubTotal = dto.SubTotal;
        TotalDiscount = dto.TotalDiscount;
        InvoiceBasedVAT = dto.InvoiceBasedVAT;
        Total = dto.Total;
        Paid = dto.CustomerPaid;
        Change = dto.Change;
        CouponCode = dto.CouponCode;
        PaymentMethod = dto.PaymentMethod;
    }

    /// <summary>
    /// Occurs when a property value changes.
    /// </summary>
    public event PropertyChangedEventHandler? PropertyChanged;

    // This event is for TrulyObservableCollection because 
    // ObservableCollection is on Microsoft's github issue
    // and can't be bind with a ListView
}
