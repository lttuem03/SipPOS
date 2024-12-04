using System.Collections.ObjectModel;
using SipPOS.DataTransfer.Entity;

namespace SipPOS.Context.Shift.Interface;

public interface IStaffShiftContext
{
    public ObservableCollection<StaffDto> OnShiftStaffs { get; set; }

    void EnShift(StaffDto staff);

    void DeShift(StaffDto staff);
}