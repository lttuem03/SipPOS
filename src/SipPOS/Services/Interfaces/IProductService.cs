using SipPOS.DataTransfer;
using SipPOS.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SipPOS.Services;

// Remove this class once your pages/features are using your data.
public interface IProductService
{
    IEnumerable<ProductDto> GetAll();

    ProductDto GetById(int id);

    ProductDto Insert(ProductDto productDto);

    ProductDto UpdateById(ProductDto productDto);

    ProductDto DeleteById(int id);

}
