using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SipPOS.Models;
using SipPOS.DataTransfer;

namespace SipPOS.DataAccess.Interfaces;

public interface ICategoryDao
{
    Pagination<Category> Search(CategoryFilterDto categoryFilterDto, SortDto sortDto, int page, int perPage);
    IList<Category> GetAll();
    Category? GetById(long id);
    Category? DeleteById(long id);
    IList<Category> DeleteByIds(IList<long> ids);
    Category? UpdateById(Category category);
    Category? Insert(Category category);
    long Count();
}
