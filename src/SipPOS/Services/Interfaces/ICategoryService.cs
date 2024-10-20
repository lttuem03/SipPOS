using SipPOS.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SipPOS.Services;

// Remove this class once your pages/features are using your data.
public interface ICategoryService
{
    IEnumerable<Category> GetAll();

    Category GetById(int id);

    Category Insert(Category product);

    Category UpdateById(Category product);

}
