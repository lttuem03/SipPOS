namespace SipPOS.DataTransfer.Entity;

/// <summary>
/// Represents an invoice data transfer object.
/// </summary>
public class InvoiceDto
{
    /// <summary>
    /// Gets or sets the invoice ID.
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Gets or sets the staff ID.
    /// </summary>
    public long StaffId { get; set; }

    /// <summary>
    /// Gets or sets the staff name.
    /// </summary>
    public string StaffName { get; set; }

    /// <summary>
    /// Gets or sets the creation date and time of the invoice.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the list of invoice items.
    /// </summary>
    public List<InvoiceItemDto> InvoiceItems { get; set; }

    /// <summary>
    /// Gets or sets the total number of items in the invoice.
    /// </summary>
    public long ItemCount { get; set; }

    /// <summary>
    /// Gets or sets the subtotal amount of the invoice.
    /// </summary>
    public decimal SubTotal { get; set; }

    /// <summary>
    /// Gets or sets the total discount amount of the invoice.
    /// </summary>
    public decimal TotalDiscount { get; set; }

    /// <summary>
    /// Gets or sets the VAT amount based on the invoice.
    /// </summary>
    public decimal InvoiceBasedVAT { get; set; }

    /// <summary>
    /// Gets or sets the total amount of the invoice.
    /// </summary>
    public decimal Total { get; set; }

    /// <summary>
    /// Gets or sets the amount paid by the customer.
    /// </summary>
    public decimal CustomerPaid { get; set; }

    /// <summary>
    /// Gets or sets the change amount to be returned to the customer.
    /// </summary>
    public decimal Change { get; set; }

    /// <summary>
    /// Gets or sets the coupon code applied to the invoice.
    /// </summary>
    public string CouponCode { get; set; }

    /// <summary>
    /// Gets or sets the payment method used for the invoice.
    /// </summary>
    public string PaymentMethod { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="InvoiceDto"/> class.
    /// </summary>
    public InvoiceDto()
    {
        Id = -1;
        StaffId = -1;
        StaffName = string.Empty;
        CreatedAt = DateTime.MinValue;
        InvoiceItems = new List<InvoiceItemDto>();
        ItemCount = 0;
        SubTotal = 0m;
        TotalDiscount = 0m;
        InvoiceBasedVAT = 0m;
        Total = 0m;
        CustomerPaid = 0m;
        Change = 0m;
        CouponCode = string.Empty;
        PaymentMethod = string.Empty;
    }
}
