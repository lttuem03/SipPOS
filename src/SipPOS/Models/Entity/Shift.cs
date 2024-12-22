using SipPOS.DataTransfer.Entity;

namespace SipPOS.Models.Entity;

/// <summary>
/// Represents a shift entity.
/// </summary>
class Shift
{
    /// <summary>
    /// Gets the unique identifier for the shift.
    /// </summary>
    public long Id
    {
        get;
    }

    /// <summary>
    /// Gets the unique identifier for the store.
    /// </summary>
    public long StoreId
    {
        get;
    }

    /// <summary>
    /// Gets the unique identifier for the staff.
    /// </summary>
    public long StaffId
    {
        get;
    }

    /// <summary>
    /// Gets the start time of the shift.
    /// </summary>
    public DateTime Start
    {
        get;
    }

    /// <summary>
    /// Gets the end time of the shift.
    /// </summary>
    public DateTime End
    {
        get;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Shift"/> class.
    /// </summary>
    /// <param name="id">The unique identifier for the shift.</param>
    /// <param name="dto">The data transfer object containing shift details.</param>
    public Shift(long id, ShiftDto dto)
    {
        Id = id;
        StoreId = dto.StoreId;
        StaffId = dto.StaffId;
        Start = dto.Start;
        End = dto.End;
    }
}
