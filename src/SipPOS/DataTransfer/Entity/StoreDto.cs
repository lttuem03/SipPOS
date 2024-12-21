namespace SipPOS.DataTransfer.Entity;

/// <summary>
/// Data transfer object for Store.
/// </summary>
public class StoreDto : BaseEntityDto
{
    /// <summary>
    /// Gets or sets the name of the store DTO.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the address of the store DTO.
    /// </summary>
    public string Address { get; set; }

    /// <summary>
    /// Gets or sets the email of the store DTO.
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// Gets or sets the telephone number of the store DTO.
    /// </summary>
    public string Tel { get; set; }

    /// <summary>
    /// Gets or sets the username of the store DTO.
    /// </summary>
    public string Username { get; set; }

    /// <summary>
    /// Gets or sets the password hash of the store DTO.
    /// </summary>
    public string? PasswordHash { get; set; }

    /// <summary>
    /// Gets or sets the salt used for hashing the password DTO.
    /// </summary>
    public string? Salt { get; set; }

    /// <summary>
    /// Gets or sets the last login date and time of the store DTO.
    /// </summary>
    public DateTime LastLogin { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="StoreDto"/> class.
    /// </summary>
    public StoreDto()
    {
        Id = null;
        CreatedBy = null;
        CreatedAt = null;
        UpdatedBy = null;
        UpdatedAt = null;
        DeletedBy = null;
        DeletedAt = null;

        Name = string.Empty;
        Address = string.Empty;
        Email = string.Empty;
        Tel = string.Empty;
        Username = string.Empty;

        PasswordHash = null;
        Salt = null;
        LastLogin = DateTime.Now; // this will always be updated correctly
                                  // due to the implementation of the Dao
                                  // that uses a database connection
    }
}
