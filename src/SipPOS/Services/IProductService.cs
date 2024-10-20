using SipPOS.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SipPOS.Services;

// Remove this class once your pages/features are using your data.
public interface IProductService
{
    Task<IEnumerable<Product>> Get();

    Task<Product> Get(int id);

    Task<Product> Add(Product product);

    Task<Product> Update(Product product);

    Task<Product> Delete(int id);

    Task<Product> Delete(Product product);

    Task<IEnumerable<Product>> GetByCategory(int categoryId);
}
