using SipPOS.Models.Entity;
using SipPOS.Services.General.Interfaces;
using SipPOS.Services.General.Implementations;
using SipPOS.Services.DataAccess.Interfaces;
using SipPOS.Services.Authentication.Interfaces;
using SipPOS.Context.Authentication;

namespace SipPOS.Services.Authentication.Implementations;

/// <summary>
/// Service for staff authentication.
/// </summary>
public class StaffAuthenticationService : IStaffAuthenticationService
{
    /// <summary>
    /// Gets the staff authentication context.
    /// </summary>
    public StaffAuthenticationContext Context { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="StaffAuthenticationService"/> class.
    /// </summary>
    public StaffAuthenticationService()
    {
        Context = new StaffAuthenticationContext();
    }

    /// <summary>
    /// Logs in a staff member asynchronously.
    /// </summary>
    /// <param name="compositeUsername">The composite username of the staff member.</param>
    /// <returns>A tuple indicating whether the login succeeded and an error message if it failed.</returns>
    public async Task<(bool succeded, string? errorMessage)> LoginAsync(string compositeUsername)
    {
        var staffDao = App.GetService<IStaffDao>();
        var staffDto = await staffDao.GetByCompositeUsernameAsync(compositeUsername);

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

        // Check if the storeId of Staff is the same as the currently authenticated store
        if (App.GetService<IStoreAuthenticationService>() is not StoreAuthenticationService storeAuthenticationService)
        {
            return (false, "Lỗi đăng nhập cửa hàng.");
        }

        if (storeAuthenticationService.Context.CurrentStore == null)
        {
            return (false, "Lỗi đăng nhập cửa hàng.");
        }


        if (staffDto.StoreId != storeAuthenticationService.Context.CurrentStore.Id)
        {
            return (false, "Nhân viên không thuộc cửa hàng này");
        }

        // LOGIN SUCCESSFUL
        var staff = new Staff(staffDto.Id.Value, staffDto);
        Context.SetStaff(staff);

        return (true, null);
    }

    /// <summary>
    /// Verifies the password of a staff member asynchronously.
    /// </summary>
    /// <param name="compositeUsername">The composite username of the staff member.</param>
    /// <param name="password">The password to verify.</param>
    /// <returns>A tuple indicating whether the verification succeeded and an error message if it failed.</returns>
    public async Task<(bool succeded, string? errorMessage)> VerifyPasswordAsync(string compositeUsername, string password)
    {
        var staffDao = App.GetService<IStaffDao>();
        var staffDto = await staffDao.GetByCompositeUsernameAsync(compositeUsername);

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

        var passwordEncryptionService = App.GetService<IPasswordEncryptionService>();

        // VERIFY PASSWORD
        if (passwordEncryptionService.Verify(password, staffDto.PasswordHash, staffDto.Salt))
        {
            return (true, null);
        }

        return (false, "Sai mật khẩu");
    }

    /// <summary>
    /// Logs out the current staff member.
    /// </summary>
    public void Logout()
    {
        if (Context.CurrentStaff != null)
        {
            Context.ClearStaff();
        }
    }
}
