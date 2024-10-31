using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SipPOS.Models;

namespace SipPOS.DataAccess.Interfaces;

public interface ICategoryDao
{
    Pagination<Category> Search(IList<object> filters, IList<object> sorts, int page, int perPage);
    IList<Category> GetAll();
    Category? GetById(long id);
    Category? DeleteById(long id);
    IList<Category> DeleteByIds(IList<long> ids);
    Category? UpdateById(Category category);
    Category? Insert(Category category);
    long Count();
}
