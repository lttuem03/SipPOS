using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SipPOS.DataTransfer.General;

/// <summary>
/// Represents a status item with a label and value.
/// </summary>
public class StatusItem
{
    /// <summary>
    /// Gets or sets the label of the status item.
    /// </summary>
    public string? Label { get; set; }

    /// <summary>
    /// Gets or sets the value of the status item.
    /// </summary>
    public string? Value { get; set; }
}
