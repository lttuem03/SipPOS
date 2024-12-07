using SipPOS.DataTransfer.Entity;

namespace SipPOS.Services.Account.Interfaces;

public interface IStaffAccountCreationService
{
    /// <summary>
    /// Creates a new staff account asynchronously.
    /// </summary>
    /// <param name="staffDto">The data transfer object containing raw staff details (the fields the user entered).</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the DTO of the created staff 
    /// if successful (to be authenticated immediately); otherwise, null.</returns>
    Task<StaffDto?> CreateAccountAsync(StaffDto staffDtoWithUnhashPassword);

    /// <summary>
    /// Validates the fields of the staff data transfer object.
    /// </summary>
    /// <param name="staffDto">The data transfer object containing staff details (the fields the user entered).</param>
    /// <returns>A dictionary containing validation results, where the key is the field name and the value is the error message or "OK".</returns>
    Dictionary<string, string> ValidateFields(StaffDto staffDto);
}