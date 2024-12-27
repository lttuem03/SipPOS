using System.ComponentModel;

namespace SipPOS.Models.Entity;

/// <summary>
/// Represents a product entity.
/// </summary>
public class Product : BaseEntity, INotifyPropertyChanged
{
    /// <summary>
    /// Gets or sets the store id of the product.
    /// </summary>
    public long StoreId { get; set; }

    /// <summary>
    /// Gets or sets the name of the product.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the description of the product.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the options of the product, each option comes with each own price.
    /// </summary>
    public List<(string name, decimal price)> ProductOptions { get; set; } = new();

    /// <summary>
    /// Gets or sets the index of the currently selected option.
    /// </summary>
    public (string name, decimal price) SelectedOption 
    { 
        get => _selectedOption;
        set
        {
            _selectedOption = value;
            OnPropertyChanged(nameof(SelectedOption));
        }
    }

    /// <summary>
    /// Gets or sets the number of items sold of the product.
    /// </summary>
    public long ItemsSold { get; set; } = 0;

    /// <summary>
    /// Gets or sets the category ID of the product.
    /// </summary>
    public long? CategoryId { get; set; }

    /// <summary>
    /// Gets or sets the list of image URLs associated with the product.
    /// </summary>
    public IList<string> ImageUris { get; set; } = new List<string>();

    /// <summary>
    /// Gets or sets the status of the product.
    /// </summary>
    public string Status { get; set; } = string.Empty;

    private (string name, decimal price) _selectedOption = ("Chưa chọn loại", 0m);

    public event PropertyChangedEventHandler? PropertyChanged;

    public void OnPropertyChanged(string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}