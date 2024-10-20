using SipPOS.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SipPOS.Services;

// Remove this class once your pages/features are using your data.
public interface IProductService
{
    IEnumerable<Product> GetAll();

    Product GetById(int id);

    Product Insert(Product product);

    Product UpdateById(Product product);

    Product DeleteById(int id);

}
