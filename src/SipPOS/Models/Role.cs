using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SipPOS.Models;

public class Role
{
    public string Name { get; set; }
    public List<Permission> Permissions { get; }

    public Role(string name)
    {
        Name = name;
        Permissions = new List<Permission>();
    }

    public void AddPermission(Permission permission)
    {
        Permissions.Add(permission);
    }
}