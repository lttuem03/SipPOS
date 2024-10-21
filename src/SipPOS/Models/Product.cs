using System;
using System.Collections.Generic;

namespace SipPOS.Models;

public class Product : BaseModel
{

    public string Name
    {
        get; set;
    }

    public string Desc
    {
        get; set;
    }

    public double Price
    {
        get; set;
    }

    public long CategoryId
    {
        get; set;
    }

    public Category Category
    {
        get; set;
    }

    public string Status
    {
        get; set;
    }

    public int SymbolCode
    {
        get; set;
    }

    public string SymbolName
    {
        get; set;
    }

    public char Symbol => (char)SymbolCode;

    public override string ToString() => $"{Name} {Desc}";
}
