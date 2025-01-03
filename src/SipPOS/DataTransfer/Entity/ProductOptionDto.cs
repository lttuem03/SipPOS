using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SipPOS.DataTransfer.Entity;

public class ProductOptionDto
{
    public string Id { get; set; } = string.Empty;
    public string? Option { get; set; }
    public decimal? Price { get; set; }
}
