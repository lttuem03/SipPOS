using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SipPOS.Models;

/// <summary>
/// Represents a role with a name and a list of permissions.
/// </summary>
public class Role
{
    /// <summary>
    /// Gets or sets the name of the role.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets the list of permissions associated with the role.
    /// </summary>
    public List<Permission> Permissions { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Role"/> class.
    /// </summary>
    /// <param name="name">The name of the role.</param>
    public Role(string name)
    {
        Name = name;
        Permissions = new List<Permission>();
    }

    /// <summary>
    /// Adds a permission to the role.
    /// </summary>
    /// <param name="permission">The permission to add.</param>
    public void AddPermission(Permission permission)
    {
        Permissions.Add(permission);
    }
}
