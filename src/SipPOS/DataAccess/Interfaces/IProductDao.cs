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
        Pagination<Product> Search(IList<object> filters, IList<object> sorts, int page, int perPage);
        IList<Product> GetAll();
        Product? GetById(long id);
        Product? DeleteById(long id);
        IList<Product> DeleteByIds(IList<long> ids);
        Product? UpdateById(Product product);
        Product? Insert(Product product);
        long Count();
    }
}
