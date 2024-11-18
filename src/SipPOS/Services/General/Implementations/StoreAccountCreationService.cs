using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Windowing;

using SipPOS.Services.DataAccess.Interfaces;
using SipPOS.Services.General.Interfaces;
using SipPOS.Models.Entity;
using SipPOS.DataTransfer.Entity;

namespace SipPOS.Services.General.Implementations;

/// <summary>
/// Service for creating store accounts.
/// </summary>
public class StoreAccountCreationService : IStoreAccountCreationService
{
    /// <summary>
    /// Creates a new store account asynchronously.
    /// </summary>
    /// <param name="storeDtoWithUnhashPassword">The store DTO with an unhashed password stored in its PasswordHash field.</param>
    /// <returns>The created store if successful (to authenticate immediately); otherwise, null.</returns>
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

        var newStore = await storeDao.InsertAsync(storeDto); // wait for the task to complete

        return newStore;
    }

    /// <summary>
    /// Validates the fields when creating a new store.
    /// </summary>
    /// <param name="storeDto">The store DTO with raw field informations to validate.</param>
    /// <returns>A dictionary containing validation results (with each field, result is "OK" or details of the
    /// failed validation).</returns>
    public Dictionary<string, string> ValidateFields(StoreDto storeDto)
    {
        var validationResults = new Dictionary<string, string>();

        validationResults.Add("name", _validateName(storeDto.Name));
        validationResults.Add("address", _validateAddress(storeDto.Address));
        validationResults.Add("email", _validateEmail(storeDto.Email));
        validationResults.Add("tel", _validateTel(storeDto.Tel));

        validationResults.Add("username", _validateUsername(storeDto.Username));
        validationResults.Add("password", _validatePassword(storeDto.PasswordHash)); // UN-HASHED PASSWORD

        // add more if needed

        return validationResults;
    }

    /// <summary>
    /// Validates the store name.
    /// </summary>
    /// <param name="name">The store name to validate.</param>
    /// <returns>A validation message.</returns>
    private string _validateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return "Tên cửa hàng không được rỗng";
        }

        return "OK";
    }

    /// <summary>
    /// Validates the store address.
    /// </summary>
    /// <param name="address">The store address to validate.</param>
    /// <returns>A validation message.</returns>
    private string _validateAddress(string address)
    {
        if (string.IsNullOrWhiteSpace(address))
        {
            return "Địa chỉ không được rỗng";
        }

        return "OK";
    }

    /// <summary>
    /// Validates the store email.
    /// </summary>
    /// <param name="email">The store email to validate.</param>
    /// <returns>A validation message.</returns>
    private string _validateEmail(string email)
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

    /// <summary>
    /// Validates the store telephone number.
    /// </summary>
    /// <param name="tel">The store telephone number to validate.</param>
    /// <returns>A validation message.</returns>
    private string _validateTel(string tel)
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

    /// <summary>
    /// Validates the store username.
    /// </summary>
    /// <param name="username">The store username to validate.</param>
    /// <returns>A validation message.</returns>
    private string _validateUsername(string username)
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

    /// <summary>
    /// Validates the store password.
    /// </summary>
    /// <param name="password">The store password to validate.</param>
    /// <returns>A validation message.</returns>
    private string _validatePassword(string? password)
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
