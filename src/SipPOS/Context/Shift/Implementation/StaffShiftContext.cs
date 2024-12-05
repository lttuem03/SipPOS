using System.Collections.ObjectModel;

using SipPOS.Context.Shift.Interface;
using SipPOS.DataTransfer.Entity;

namespace SipPOS.Context.Shift.Implementation;

public class StaffShiftContext : IStaffShiftContext
{
    public ObservableCollection<StaffDto> OnShiftStaffs { get; set; }
    
    private StaffDto NoStaffDto { get; }

    public StaffShiftContext()
    {
        OnShiftStaffs = new();
        NoStaffDto = new()
        {
            Name = "(chưa có nhân viên trong ca)",
            PositionPrefix = "NO_STAFF_IN_SHIFT_",
            StoreId = 0,
            Id = 0
        };

        OnShiftStaffs.Add(NoStaffDto);

        OnShiftStaffs.Add(new StaffDto() 
        { 
            Name = "Trần Nhật Minh",
            PositionPrefix = "ST",
            StoreId = 0,
            Id = 3
        });
    }

    public void EnShift(StaffDto staff)
    {
        OnShiftStaffs.Add(staff);
    }

    public void DeShift(StaffDto staff)
    {
        OnShiftStaffs.Remove(staff);
    }
}