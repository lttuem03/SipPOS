using SipPOS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SipPOS.DataAccess.Interfaces
{
    public interface IProductDao
    {
        IEnumerable<Product> GetAll();
        Product GetById(long Id);
        Product DeleteById(long Id);
        Product UpdateById(Product product);
        Product Insert(Product product);

    }
}
