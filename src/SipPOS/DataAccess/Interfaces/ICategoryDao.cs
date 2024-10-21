using SipPOS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SipPOS.DataAccess.Interfaces
{
    public interface ICategoryDao
    {
        IEnumerable<Category> GetAll();
        Category GetById(long Id);
        Category DeleteById(long Id);
        Category UpdateById(Category category);
        Category Insert(Category category);
    }
}
