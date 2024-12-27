using System.ComponentModel;

namespace SipPOS.DataTransfer.Entity;

public class InvoiceItemDto : INotifyPropertyChanged
{
    private long _id = -1;
    private long _invoiceId = -1;
    private long _productId = -1;
    private long _ordinal = -1;
    private string _itemName = string.Empty;
    private string _optionName = string.Empty;
    private decimal _optionPrice = 0m;
    private decimal _discount = 0m;
    private string _note = string.Empty;

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public long Id
    {
        get => _id;
        set
        {
            _id = value;
            OnPropertyChanged(nameof(Id));
        }
    }

    public long InvoiceId
    {
        get => _invoiceId;
        set
        {
            _invoiceId = value;
            OnPropertyChanged(nameof(InvoiceId));
        }
    }

    public long ProductId
    {
        get => _productId;
        set
        {
            _productId = value;
            OnPropertyChanged(nameof(ProductId));
        }
    }

    public long Ordinal
    {
        get => _ordinal;
        set
        {
            _ordinal = value;
            OnPropertyChanged(nameof(Ordinal));
        }
    }

    public string ItemName
    {
        get => _itemName;
        set
        {
            _itemName = value;
            OnPropertyChanged(nameof(ItemName));
        }
    }

    public string OptionName
    {
        get => _optionName;
        set
        {
            _optionName = value;
            OnPropertyChanged(nameof(OptionName));
        }
    }

    public decimal OptionPrice
    {
        get => _optionPrice;
        set
        {
            _optionPrice = value;
            OnPropertyChanged(nameof(OptionPrice));
        }
    }

    public decimal Discount
    {
        get => _discount;
        set
        {
            _discount = value;
            OnPropertyChanged(nameof(Discount));
        }
    }

    public string Note
    {
        get => _note;
        set
        {
            _note = value;
            OnPropertyChanged(nameof(Note));
        }
    }
}