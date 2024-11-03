using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SipPOS.DataTransfer;

namespace SipPOS.Models;

public class Store : BaseModel
{
    public string Name { get; }
    public string Address { get; }
    public string Email { get; }
    public string Tel { get; }
    public string Username { get; }
    public DateTime LastLogin { get; }

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