using System.Collections.ObjectModel;
using SipPOS.DataTransfer.Entity;

namespace SipPOS.Context.Shift.Interface;

/// <summary>
/// Interface for managing staff shifts.
/// </summary>
public interface IStaffShiftContext
{
    /// <summary>
    /// Gets or sets the collection of staff currently on shift.
    /// </summary>
    public ObservableCollection<StaffDto> OnShiftStaffs { get; set; }

    /// <summary>
    /// Adds a staff member to the shift.
    /// </summary>
    /// <param name="staff">The staff member to add.</param>
    void EnShift(StaffDto staff);

    /// <summary>
    /// Removes a staff member from the shift.
    /// </summary>
    /// <param name="staff">The staff member to remove.</param>
    void DeShift(StaffDto staff);
}
