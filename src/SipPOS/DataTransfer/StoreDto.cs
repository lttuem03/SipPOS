using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SipPOS.DataTransfer;

public class StoreDto : BaseDto
{
    public string Name { get; set; }
    public string Address { get; set; }
    public string Email { get; set; }
    public string Tel { get; set; }
    public string Username { get; set; }
    public string? PasswordHash { get; set; }
    public string? Salt { get; set; }
    public DateTime LastLogin { get; set; }

    public StoreDto()
    {
        Id = null;
        CreatedBy = null;
        CreatedAt = null;
        UpdatedBy = null;
        UpdatedAt = null;
        DeletedBy = null;
        DeletedAt = null;

        Name = String.Empty;
        Address = String.Empty;
        Email = String.Empty;
        Tel = String.Empty;
        Username = String.Empty;

        PasswordHash = null;
        Salt = null;
        LastLogin = DateTime.Now; // this will always be updated correctly
                                  // due to the implementation of the Dao
                                  // that uses a database connection
    }
}