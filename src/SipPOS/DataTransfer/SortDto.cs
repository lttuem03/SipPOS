using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SipPOS.DataTransfer
{
    public class SortDto
    {
        public string? SortBy { get; set; } = "CreatedAt";
        public string? SortType { get; set; } = "Asc";
    }
}
