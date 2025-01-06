using System.Collections.ObjectModel;

using SipPOS.Context.Shift.Interface;
using SipPOS.DataTransfer.Entity;

namespace SipPOS.Context.Shift.Implementation;

/// <summary>
/// Represents the context for managing staff shifts.
/// </summary>
public class StaffShiftContext : IStaffShiftContext
{
    /// <summary>
    /// Gets or sets the collection of staff currently on shift.
    /// </summary>
    public ObservableCollection<StaffDto> OnShiftStaffs { get; set; }

    /// <summary>
    /// Gets the default staff DTO representing no staff on shift.
    /// </summary>
    private StaffDto NoStaffDto { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="StaffShiftContext"/> class.
    /// </summary>
    public StaffShiftContext()
    {
        OnShiftStaffs = new();
        NoStaffDto = new();
    }

    /// <summary>
    /// Adds a staff member to the shift.
    /// </summary>
    /// <param name="staff">The staff member to add.</param>
    public void EnShift(StaffDto staff)
    {
        OnShiftStaffs.Add(staff);
    }

    /// <summary>
    /// Removes a staff member from the shift.
    /// </summary>
    /// <param name="staff">The staff member to remove.</param>
    public void DeShift(StaffDto staff)
    {
        OnShiftStaffs.Remove(staff);
    }
}
