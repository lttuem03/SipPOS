using System.ComponentModel;

namespace SipPOS.DataTransfer.Entity;

/// <summary>
/// Represents an invoice item data transfer object.
/// </summary>
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

    /// <summary>
    /// Occurs when a property value changes.
    /// </summary>
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    /// Raises the PropertyChanged event.
    /// </summary>
    /// <param name="propertyName">Name of the property that changed.</param>
    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    /// <summary>
    /// Raises the PropertyChanged event.
    /// </summary>
    /// <param name="args">The PropertyChangedEventArgs instance containing the event data.</param>
    private void OnPropertyChanged(PropertyChangedEventArgs args)
    {
        PropertyChanged?.Invoke(this, args);
    }

    /// <summary>
    /// Gets or sets the ID of the invoice item.
    /// </summary>
    public long Id
    {
        get => _id;
        set
        {
            _id = value;
            OnPropertyChanged(nameof(Id));
        }
    }

    /// <summary>
    /// Gets or sets the invoice ID to which this item belongs.
    /// </summary>
    public long InvoiceId
    {
        get => _invoiceId;
        set
        {
            _invoiceId = value;
            OnPropertyChanged(nameof(InvoiceId));
        }
    }

    /// <summary>
    /// Gets or sets the product ID of the invoice item.
    /// </summary>
    public long ProductId
    {
        get => _productId;
        set
        {
            _productId = value;
            OnPropertyChanged(nameof(ProductId));
        }
    }

    /// <summary>
    /// Gets or sets the ordinal position of the invoice item.
    /// </summary>
    public long Ordinal
    {
        get => _ordinal;
        set
        {
            _ordinal = value;
            OnPropertyChanged(nameof(Ordinal));
        }
    }

    /// <summary>
    /// Gets or sets the name of the item.
    /// </summary>
    public string ItemName
    {
        get => _itemName;
        set
        {
            _itemName = value;
            OnPropertyChanged(nameof(ItemName));
        }
    }

    /// <summary>
    /// Gets or sets the name of the option.
    /// </summary>
    public string OptionName
    {
        get => _optionName;
        set
        {
            _optionName = value;
            OnPropertyChanged(nameof(OptionName));
        }
    }

    /// <summary>
    /// Gets or sets the price of the option.
    /// </summary>
    public decimal OptionPrice
    {
        get => _optionPrice;
        set
        {
            _optionPrice = value;
            OnPropertyChanged(nameof(OptionPrice));
        }
    }

    /// <summary>
    /// Gets or sets the discount applied to the item.
    /// </summary>
    public decimal Discount
    {
        get => _discount;
        set
        {
            _discount = value;
            OnPropertyChanged(nameof(Discount));
        }
    }

    /// <summary>
    /// Gets or sets the note associated with the item.
    /// </summary>
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
