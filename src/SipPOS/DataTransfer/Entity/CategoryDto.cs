using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SipPOS.DataTransfer.Entity;

/// <summary>
/// Data transfer object for Category.
/// </summary>
public class CategoryDto : BaseEntityDto
{
    /// <summary>
    /// Gets or sets the name of the category.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the description of the category.
    /// </summary>
    public string? Desc { get; set; }

    /// <summary>
    /// Gets or sets the status of the category.
    /// </summary>
    public string? Status { get; set; }

    /// <summary>
    /// Gets or sets the list of image URLs associated with the category.
    /// </summary>
    public IList<string> ImageUrls { get; set; } = new List<string>();

    /// <summary>
    /// Gets or sets a value indicating whether the category is settled.
    /// </summary>
    public bool IsSeteled { get; set; } = false;
}
