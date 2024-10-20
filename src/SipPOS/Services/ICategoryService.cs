using SipPOS.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SipPOS.Services;

// Remove this class once your pages/features are using your data.
public interface ICategoryService
{
    Task<IEnumerable<Category>> Get();

    Task<Category?> Get(int id);

    Task<Category> Add(Category product);

    Task<Category> Update(Category product);

}
