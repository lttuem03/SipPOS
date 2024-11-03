using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SipPOS.Models;
using SipPOS.DataTransfer;
using SipPOS.Services.Interfaces;
using SipPOS.DataAccess.Interfaces;
using Microsoft.UI.Windowing;

namespace SipPOS.Services.Implementations;
public class StoreAccountCreationService : IStoreAccountCreationService
{
    public async Task<Store?> CreateAccountAsync(StoreDto storeDtoWithUnhashPassword)
    {
        if (storeDtoWithUnhashPassword.PasswordHash == null) // UN-HASHED PASSWORD
        {
            return null;
        }

        // ASSUMING ALL VALIDATIONS PASSED        
        
        // Hash password
        var passwordEncryptionService = App.GetService<IPasswordEncryptionService>();

        (var passwordHash, var salt) = passwordEncryptionService.Hash(storeDtoWithUnhashPassword.PasswordHash);

        storeDtoWithUnhashPassword.PasswordHash = passwordHash;
        storeDtoWithUnhashPassword.Salt = salt;
        storeDtoWithUnhashPassword.CreatedBy = "system";
        storeDtoWithUnhashPassword.CreatedAt = DateTime.Now;

        var storeDto = storeDtoWithUnhashPassword; // now-hased, ready

        // Insert new row to database
        var storeDao = App.GetService<IStoreDao>();
        
        Store? newStore = await storeDao.InsertAsync(storeDto); // wait for the task to complete

        return newStore;
    }
    
    public Dictionary<string, string> ValidateFields(StoreDto storeDto)
    {
        var validationResults = new Dictionary<string, string>();

        validationResults.Add("name", validateName(storeDto.Name));
        validationResults.Add("address", validateAddress(storeDto.Address));
        validationResults.Add("email", validateEmail(storeDto.Email));
        validationResults.Add("tel", validateTel(storeDto.Tel));
        
        validationResults.Add("username", validateUsername(storeDto.Username));
        validationResults.Add("password", validatePassword(storeDto.PasswordHash)); // UN-HASHED PASSWORD
        
        // add more if needed

        return validationResults;
    }

    private string validateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return "Tên cửa hàng không được rỗng";
        }

        return "OK";
    }

    private string validateAddress(string address)
    {
        if (string.IsNullOrWhiteSpace(address))
        {
            return "Địa chỉ không được rỗng";
        }

        return "OK";
    }

    private string validateEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return "Email không được rỗng";
        }

        var emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

        if (System.Text.RegularExpressions.Regex.IsMatch(email, emailPattern))
        {
            return "OK";
        }

        return "Email sai định dạng";
    }

    private string validateTel(string tel)
    {
        if (string.IsNullOrWhiteSpace(tel))
        {
            return "Số điện thoại không được rỗng";
        }

        var telPattern = @"^(0|\+84)\d{9,12}$";

        if (System.Text.RegularExpressions.Regex.IsMatch(tel, telPattern))
        {
            return "OK";
        }

        return "Số điện thoại sai định dạng";
    }

    private string validateUsername(string username)
    {
        if (string.IsNullOrWhiteSpace(username))
        {
            return "Tên đăng nhập không được rỗng";
        }

        var storeDao = App.GetService<IStoreDao>();
        
        var storeWithUsernameExists = storeDao.GetByUsernameAsync(username).Result;

        if (storeWithUsernameExists != null)
        {
            return "Tên đăng nhập đã tồn tại";
        }

        return "OK";
    }

    private string validatePassword(string? password)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            return "Mật khẩu không được rỗng";
        }

        if (password.Length < 8)
        {
            return "Mật khẩu không được ngắn hơn 8 kí tự";
        }

        if (password.Length >= 32)
        {
            return "Mật khẩu không được dài quá 32 kí tự";
        }

        return "OK";
    }
}