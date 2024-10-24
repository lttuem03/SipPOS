using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SipPOS.DataTransfer
{
    public class ProductDto : BaseDto
    {
        public string? Name { get; set; }
        public string? Desc { get; set; }
        public double? Price { get; set; }
        public IList<string> ImageUrls { get; set; } = new List<string>();
        public long? CategoryId { get; set; }
        public string? Status { get; set; }

        public bool IsSeteled { get; set; } = false;
    }
}
