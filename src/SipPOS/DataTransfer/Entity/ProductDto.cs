using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SipPOS.DataTransfer.Entity;

/// <summary>
/// Data transfer object for Product.
/// </summary>
public class ProductDto : BaseDto
{
    /// <summary>
    /// Gets or sets the name of the product.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the description of the product.
    /// </summary>
    public string? Desc { get; set; }

    /// <summary>
    /// Gets or sets the price of the product.
    /// </summary>
    public double? Price { get; set; }

    /// <summary>
    /// Gets or sets the list of image URLs associated with the product.
    /// </summary>
    public IList<string> ImageUrls { get; set; } = new List<string>();

    /// <summary>
    /// Gets or sets the category ID of the product.
    /// </summary>
    public long? CategoryId { get; set; }

    /// <summary>
    /// Gets or sets the status of the product.
    /// </summary>
    public string? Status { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the product is settled.
    /// </summary>
    public bool IsSeteled { get; set; } = false;

    /// <summary>
    /// Gets or sets the quantity of the product.
    /// </summary>
    public int? Quantity { get; set; }
}
