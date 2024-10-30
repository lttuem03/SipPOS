using System;
using System.Collections.Generic;

namespace SipPOS.Models;

public class Product : BaseModel
{
    public string? Name { get; set; }
    
    public string? Desc { get; set; }
    
    public double? Price { get; set; }
    
    public long? CategoryId { get; set; }

    public IList<string>  ImageUrls { get; set; } = new List<string>();

    public string? Status { get; set; }

}
