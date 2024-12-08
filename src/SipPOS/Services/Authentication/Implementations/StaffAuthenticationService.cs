using SipPOS.Context.Authentication;
using SipPOS.DataTransfer.Entity;
using SipPOS.Services.DataAccess.Interfaces;
using SipPOS.Services.Authentication.Interfaces;
using SipPOS.Models.Entity;

namespace SipPOS.Services.Authentication.Implementations;

public class StaffAuthenticationService : IStaffAuthenticationService
{
    public StaffAuthenticationContext Context { get; }

    public StaffAuthenticationService()
    {
        Context = new StaffAuthenticationContext();
    }

    public async Task<(bool succeded, string? errorMessage)> LoginAsync(string compositeUsername)
    {
        var staffDao = App.GetService<IStaffDao>();
        var staffDto = await staffDao.GetByCompositeUsername(compositeUsername);

        if (staffDto == null) // staff not exists in database
        {
            return (false, "Nhân viên không tồn tại");
        }

        // check if the staff returned had been marked as "Deleted" or not
        if (staffDto.DeletedBy != null)
        {
            return (false, "Tài khoản nhân viên đã bị xóa");
        }

        // check if the staff returned had been marked as "OutOfEmployment" or not
        if (staffDto.EmploymentStatus == "OutOfEmployment")
        {
            return (false, "Nhân viên đã kết thúc hợp đồng công việc");
        }

        // null checks
        if (staffDto.Id == null)
        {
            return (false, "Lỗi chưa xác định");
        }

        // remark: A staff can be authenticated using only their composite username.
        // Their password is used to open shifts.

        // LOGIN SUCCESSFUL
        var staff = new Staff(staffDto.Id.Value, staffDto);
        Context.SetStaff(staff);
        
        return (true, null);
    }

    public void Logout()
    {
        if (Context.CurrentStaff != null)
        {
            Context.ClearStaff();
        }
    }
}