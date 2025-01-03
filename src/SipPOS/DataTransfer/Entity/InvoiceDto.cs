using System.Collections.ObjectModel;

namespace SipPOS.DataTransfer.Entity;

public class InvoiceDto
{
    public long Id { get; set; }

    public long StaffId { get; set; }

    public string StaffName { get; set; }

    public DateTime CreatedAt { get; set; }

    public List<InvoiceItemDto> InvoiceItems { get; set; }

    public long ItemCount { get; set; }

    public decimal SubTotal { get; set; }
    
    public decimal TotalDiscount { get; set; }

    public decimal InvoiceBasedVAT { get; set; }

    public decimal Total { get; set; }

    public decimal CustomerPaid { get; set; }

    public decimal Change { get; set; }

    public string PaymentMethod { get; set; }

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
        PaymentMethod = string.Empty;
    }
}