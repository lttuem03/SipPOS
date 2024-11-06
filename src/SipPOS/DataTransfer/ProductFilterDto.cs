using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SipPOS.DataTransfer
{
    public class ProductFilterDto
    {
        public string? Name { get; set; }
        public string? Desc { get; set; }
        public long? CategoryId { get; set; }
        public string? Status { get; set; }
    }
}
