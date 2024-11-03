using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SipPOS.Models;
using SipPOS.DataTransfer;
using SipPOS.DataAccess.Implementations;

namespace SipPOS.DataAccess.Interfaces;

public interface IProductDao
{
    Pagination<Product> Search(ProductFilterDto productFilterDto, SortDto sortDto, int page, int perPage);
    IList<Product> GetAll();
    Product? GetById(long id);
    Product? DeleteById(long id);
    IList<Product> DeleteByIds(IList<long> ids);
    Product? UpdateById(Product product);
    Product? Insert(Product product);
    long Count();
}
