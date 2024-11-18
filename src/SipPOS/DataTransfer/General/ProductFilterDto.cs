using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SipPOS.DataTransfer.General;

/// <summary>
/// Data transfer object for filtering products.
/// </summary>
public class ProductFilterDto
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
    /// Gets or sets the category ID of the product.
    /// </summary>
    public long? CategoryId { get; set; }

    /// <summary>
    /// Gets or sets the status of the product.
    /// </summary>
    public string? Status { get; set; }
}
