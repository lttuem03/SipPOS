using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SipPOS.Models;

public class Permission
{
    public string Name { get; set; }

    Permission(string name)
    {
        Name = name;
    }
}