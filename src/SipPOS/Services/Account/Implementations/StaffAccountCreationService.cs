using System.Text.RegularExpressions;

using SipPOS.DataTransfer.Entity;
using SipPOS.Services.Account.Interfaces;
using SipPOS.Services.Authentication.Implementations;
using SipPOS.Services.Authentication.Interfaces;
using SipPOS.Services.General.Implementations;
using SipPOS.Services.General.Interfaces;
using SipPOS.Services.DataAccess.Interfaces;

namespace SipPOS.Services.Account.Implementations;

class StaffAccountCreationService : IStaffAccountCreationService
{
    public async Task<StaffDto?> CreateAccountAsync(StaffDto staffDtoWithUnhashPassword)
    {
        if (staffDtoWithUnhashPassword.PasswordHash == null)
        {
            return null;
        }

        long storeId = -1;

        if (App.GetService<IStoreAuthenticationService>() is StoreAuthenticationService storeAuthenticationService)
        {
            // It makes no sense if a staff account can be created while no store is logged in
            if (storeAuthenticationService.Context.LoggedIn == false)
                return null;

            if (storeAuthenticationService.Context.CurrentStore == null)
                return null;

            storeId = storeAuthenticationService.Context.CurrentStore.Id;
        }

        // ASSUMING ALL VALIDATIONS PASSED

        // Hash password
        var passwordEncryptionService = App.GetService<IPasswordEncryptionService>();

        (var passwordHash, var salt) = passwordEncryptionService.Hash(staffDtoWithUnhashPassword.PasswordHash);

        staffDtoWithUnhashPassword.PasswordHash = passwordHash;
        staffDtoWithUnhashPassword.Salt = salt;
        staffDtoWithUnhashPassword.CreatedAt = DateTime.Now;

        if (App.GetService<IStaffAuthenticationService>() is StaffAuthenticationService staffAuthenticationService)
        {
            if (staffAuthenticationService.Context.CurrentStaff != null)
            {
                staffDtoWithUnhashPassword.CreatedBy = staffAuthenticationService.Context.CurrentStaff.CompositeUsername;
            }
        }
        
        staffDtoWithUnhashPassword.CreatedBy ??= "system";
        staffDtoWithUnhashPassword.EmploymentStatus = "InEmployment";

        var staffDto = staffDtoWithUnhashPassword; // now-hased, ready
        
        var staffDao = App.GetService<IStaffDao>();
        var insertResult = await staffDao.InsertAsync
        (
            storeId: storeId,
            staffDto: staffDto
        );

        if (insertResult.dto == null)
        {
            return null;
        }

        var insertedStaffDto = insertResult.dto;
        insertedStaffDto.Id = insertResult.id;

        return insertedStaffDto;
    }

    public Dictionary<string, string> ValidateFields(StaffDto staffDto)
    {
        var validationResults = new Dictionary<string, string>();

        validationResults.Add("name", _validateName(staffDto.Name));
        validationResults.Add("gender", _validateGender(staffDto.Gender));
        validationResults.Add("email", _validateEmail(staffDto.Email));
        validationResults.Add("tel", _validateTel(staffDto.Tel));
        validationResults.Add("address", _validateAddress(staffDto.Address));
        validationResults.Add("password", _validatePassword(staffDto.PasswordHash));

        // add more if needed

        return validationResults;
    }

    private string _validateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return "Tên không được rỗng";
        }

        return "OK";
    }

    private string _validateGender(string gender)
    {
        if (string.IsNullOrWhiteSpace(gender))
        {
            return "Giới tính không được rỗng";
        }

        if (gender != "Nam" && gender != "Nữ")
        {
            return "Giới tính phải thuộc giá trị 'Nam' hoặc 'Nữ'";
        }

        return "OK";
    }

    private string _validateEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return "Email không được rỗng";
        }

        var emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

        if (Regex.IsMatch(email, emailPattern))
        {
            return "OK";
        }

        return "Email sai định dạng";
    }

    private string _validateTel(string tel)
    {
        if (string.IsNullOrWhiteSpace(tel))
        {
            return "SĐT không được rỗng";
        }

        var telPattern = @"^(0|\+84)\d{9,12}$";

        if (System.Text.RegularExpressions.Regex.IsMatch(tel, telPattern))
        {
            return "OK";
        }

        return "Số điện thoại sai định dạng";
    }

    private string _validateAddress(string address)
    {
        if (string.IsNullOrWhiteSpace(address))
        {
            return "Địa chỉ không được rỗng";
        }

        return "OK";
    }

    private string _validatePassword(string? password)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            return "Mật khẩu không được rỗng";
        }

        if (password.Length < 6)
        {
            return "Mật khẩu không được ngắn hơn 6 kí tự";
        }

        if (password.Length >= 32)
        {
            return "Mật khẩu không được dài quá 32 kí tự";
        }

        return "OK";
    }
}