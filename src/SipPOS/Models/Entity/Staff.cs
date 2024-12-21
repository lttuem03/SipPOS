using SipPOS.DataTransfer.Entity;
using SipPOS.Models.General;

namespace SipPOS.Models.Entity;

/// <summary>
/// Represents a staff entity.
/// </summary>
public class Staff : BaseEntity
{
    /// <summary>
    /// The regex pattern that can match a composite username.
    /// Currently it can only match one of the three position prefixes:
    /// ST: Staff, AM: Assistant Manager, SM: Store Manager.
    /// Followed by the store id, and then the staff id (each of which has 3 digits).
    /// </summary>
    public static readonly string CompositeUsernamePattern = @"^(ST|AM|SM)(\d{3})(\d{3})$";

    /// <summary>
    /// The composite username used for authentication.
    /// </summary>
    public string CompositeUsername => GetCompositeUsername(PositionPrefix, StoreId, Id);

    /// <summary>
    /// Gets the store identifier.
    /// </summary>
    public long StoreId
    {
        get;
    }

    /// <summary>
    /// Get the position object assigned to this staff (based on the prefix)
    /// </summary>
    public Position Position
    {
        get; private set;
    }

    /// <summary>
    /// Gets the position prefix.
    /// </summary>
    public string PositionPrefix
    {
        get => _positionPrefix;
        private set
        {
            _positionPrefix = value;
            Position = Position.FromPrefix(value);
        }
    }

    /// <summary>
    /// Gets the name of the staff.
    /// </summary>
    public string Name
    {
        get;
    }

    /// <summary>
    /// Gets the gender of the staff.
    /// </summary>
    public string Gender
    {
        get;
    }

    /// <summary>
    /// Gets the date of birth of the staff.
    /// </summary>
    public DateOnly DateOfBirth
    {
        get;
    }

    /// <summary>
    /// Gets the email of the staff.
    /// </summary>
    public string Email
    {
        get;
    }

    /// <summary>
    /// Gets the telephone number of the staff.
    /// </summary>
    public string Tel
    {
        get;
    }

    /// <summary>
    /// Gets the address of the staff.
    /// </summary>
    public string Address
    {
        get;
    }

    /// <summary>
    /// Gets the employment status of the staff.
    /// </summary>
    public string EmploymentStatus
    {
        get;
    }

    /// <summary>
    /// Gets the employment start date.
    /// </summary>
    public DateOnly EmploymentStartDate
    {
        get;
    }

    /// <summary>
    /// Gets the employment end date.
    /// </summary>
    public DateOnly? EmploymentEndDate
    {
        get;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Staff"/> class.
    /// </summary>
    /// <param name="id">The unique identifier of the staff.</param>
    /// <param name="dto">The data transfer object containing staff details.</param>
    public Staff(long id, StaffDto dto)
    {
        _positionPrefix = "ST";

        Id = id;
        StoreId = dto.StoreId;
        Position = Position.Staff; // default, might change when assigning prefix
        PositionPrefix = dto.PositionPrefix;

        Name = dto.Name;
        Gender = dto.Gender;
        DateOfBirth = dto.DateOfBirth;
        Email = dto.Email;
        Tel = dto.Tel;
        Address = dto.Address;
        EmploymentStatus = dto.EmploymentStatus;
        EmploymentStartDate = dto.EmploymentStartDate;
        EmploymentEndDate = dto.EmploymentEndDate;

        CreatedAt = dto.CreatedAt;
        CreatedBy = dto.CreatedBy;
        UpdatedAt = dto.UpdatedAt;
        UpdatedBy = dto.UpdatedBy;
        DeletedAt = dto.DeletedAt;
        DeletedBy = dto.DeletedBy;
    }

    /// <summary>
    /// Generates a composite username from the components.
    /// The username is composed of the position prefix, store ID, and staff ID.
    /// (Eg. ST001002, SM002001)
    /// </summary>
    /// <param name="positionPrefix">The position prefix of the staff.</param>
    /// <param name="storeId">The store identifier.</param>
    /// <param name="staffId">The staff identifier.</param>
    /// <returns>A composite username string.</returns>
    public static string GetCompositeUsername(string positionPrefix, long storeId, long staffId)
    {
        return $"{positionPrefix}{storeId.ToString("000")}{staffId.ToString("000")}";
    }

    /// <summary>
    /// Backup field for PositionPrefix.
    /// </summary>
    private string _positionPrefix;
}