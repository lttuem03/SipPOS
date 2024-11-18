using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SipPOS.Models.General;

/// <summary>
/// Represents a permission with a name.
/// </summary>
public class Permission
{
    /// <summary>
    /// Gets or sets the name of the permission.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Permission"/> class.
    /// </summary>
    /// <param name="name">The name of the permission.</param>
    public Permission(string name)
    {
        Name = name;
    }
}
