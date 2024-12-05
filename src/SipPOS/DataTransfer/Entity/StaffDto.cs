using SipPOS.Models.General;

namespace SipPOS.DataTransfer.Entity;

/// <summary>
/// Data Transfer Object for Staff entity.
/// </summary>
public class StaffDto : BaseEntityDto
{
    /// <summary>
    /// Gets or sets the store identifier.
    /// </summary>
    public long StoreId { get; set; }

    /// <summary>
    /// Gets or privately sets the Position (can only be changed when the PositionPrefix changes)
    /// </summary>
    public Position Position { get; private set; }

    /// <summary>
    /// Gets or sets the position prefix.
    /// </summary>
    public string PositionPrefix
    {
        get => _positionPrefix;
        set
        {
            _positionPrefix = value;
            Position = Position.FromPrefix(value);
        }
    }

    /// <summary>
    /// The composite username used for authentication.
    /// </summary>
    public string CompositeUsername
    {
        get
        {
            if (Id.HasValue)
                return $"{PositionPrefix}{StoreId.ToString("000")}{Id.Value.ToString("000")}";
            else
                return "Id not set";
        }
    }

    /// <summary>
    /// Gets or sets the password hash.
    /// </summary>
    public string PasswordHash { get; set; }

    /// <summary>
    /// Gets or sets the salt used for hashing the password.
    /// </summary>
    public string Salt { get; set; }

    /// <summary>
    /// Gets or sets the name of the staff.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the date of birth of the staff.
    /// </summary>
    public DateOnly DateOfBirth { get; set; }

    /// <summary>
    /// Gets or sets the email of the staff.
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// Gets or sets the telephone number of the staff.
    /// </summary>
    public string Tel { get; set; }

    /// <summary>
    /// Gets or sets the address of the staff.
    /// </summary>
    public string Address { get; set; }

    /// <summary>
    /// Gets or sets the employment status of the staff.
    /// Possible values: "In employment", "Out of employment".
    /// </summary>
    public string EmploymentStatus { get; set; }

    /// <summary>
    /// Gets or sets the employment start date.
    /// </summary>
    public DateOnly EmploymentStartDate { get; set; }

    /// <summary>
    /// Gets or sets the employment end date.
    /// </summary>
    public DateOnly? EmploymentEndDate { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="StaffDto"/> class.
    /// Sets default values for the properties.
    /// </summary>
    public StaffDto()
    {
        _positionPrefix = "ST";

        Id = -1;
        StoreId = -1;
        Position = Position.Staff; // default, might change when assigning prefix
        PositionPrefix = string.Empty;
        PasswordHash = string.Empty;
        Salt = string.Empty;
        Name = string.Empty;
        DateOfBirth = DateOnly.MinValue;
        Email = string.Empty;
        Tel = string.Empty;
        Address = string.Empty;
        EmploymentStatus = string.Empty;

        EmploymentStartDate = DateOnly.MinValue;
        EmploymentEndDate = null;

        CreatedAt = null;
        CreatedBy = null;
        UpdatedAt = null;
        UpdatedBy = null;
        DeletedAt = null;
        DeletedBy = null;
    }
    
    /// <summary>
    /// Backup field for PositionPrefix.
    /// </summary>
    private string _positionPrefix;
}
