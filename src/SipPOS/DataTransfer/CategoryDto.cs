using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SipPOS.DataTransfer
{
    public class CategoryDto : BaseDto
    {
        public long Id { get; set; }
        public string Name { get; set; }

        public string Desc { get; set; }

    }
}
