using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SipPOS.DataTransfer.Entity;

/// <summary>
/// Data Transfer Object for Shift.
/// </summary>
public class ShiftDto
{
    /// <summary>
    /// Gets or sets the unique identifier for the shift.
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier for the store.
    /// </summary>
    public long StoreId { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier for the staff.
    /// </summary>
    public long StaffId { get; set; }

    /// <summary>
    /// Gets or sets the start time of the shift.
    /// </summary>
    public DateTime Start { get; set; }

    /// <summary>
    /// Gets or sets the end time of the shift.
    /// </summary>
    public DateTime End { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ShiftDto"/> class.
    /// </summary>
    public ShiftDto()
    {
        Id = -1;
        StoreId = -1;
        StaffId = -1;
        Start = DateTime.MinValue;
        End = DateTime.MaxValue;
    }
}
