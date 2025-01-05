using System.Collections.ObjectModel;
using SipPOS.DataTransfer.Entity;
using System.ComponentModel;

namespace SipPOS.Models.Entity;

public class Invoice : INotifyPropertyChanged
{
    public long Id { get; }

    public long StaffId { get; }

    public string StaffName { get; }

    public DateTime CreatedAt { get; }

    public List<InvoiceItem> InvoiceItems { get; }

    public long ItemCount { get; }

    public decimal SubTotal { get; }
    
    public decimal TotalDiscount { get; }

    public decimal InvoiceBasedVAT { get; }

    public decimal Total { get; }

    public decimal Paid { get; }

    public decimal Change { get; }

    public string PaymentMethod { get; }

    public Invoice(InvoiceDto dto)
    {
        Id = dto.Id;
        StaffId = dto.StaffId;
        StaffName = dto.StaffName;
        CreatedAt = dto.CreatedAt;

        InvoiceItems = new();
        foreach(var invoiceItemDto in dto.InvoiceItems)
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
        PaymentMethod = dto.PaymentMethod;
    }

    // To use TrulyObservableCollection because 
    // ObservableCollection is on Microsoft's github issue
    // and can't be bind with a ListView
    public event PropertyChangedEventHandler? PropertyChanged;
}