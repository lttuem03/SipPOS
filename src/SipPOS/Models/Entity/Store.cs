using SipPOS.DataTransfer.Entity;

namespace SipPOS.Models.Entity;

/// <summary>
/// Represents a store entity.
/// </summary>
public class Store : BaseEntity
{
    /// <summary>
    /// Gets the name of the store.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the address of the store.
    /// </summary>
    public string Address { get; }

    /// <summary>
    /// Gets the email of the store.
    /// </summary>
    public string Email { get; }

    /// <summary>
    /// Gets the telephone number of the store.
    /// </summary>
    public string Tel { get; }

    /// <summary>
    /// Gets the username associated with the store.
    /// </summary>
    public string Username { get; }

    /// <summary>
    /// Gets the last login date and time of the store.
    /// </summary>
    public DateTime LastLogin { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Store"/> class.
    /// </summary>
    /// <param name="id">The unique identifier of the store.</param>
    /// <param name="dto">The data transfer object containing store details.</param>
    public Store(long id, StoreDto dto)
    {
        Id = id;
        Name = dto.Name;
        Address = dto.Address;
        Email = dto.Email;
        Tel = dto.Tel;
        Username = dto.Username;
        LastLogin = dto.LastLogin;
    }
}
