using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SipPOS.Models
{
    public class Pagination<T>
    {
        public int Page { get; set; } = 1;
        public int TotalPage { get; set; } = 1;
        public int PerPage { get; set; } = 5;
        public long TotalRecord { get; set; } = 0;
        public IList<T> Data { get; set; } = new List<T>();

    }
}
