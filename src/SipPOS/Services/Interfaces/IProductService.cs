using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

using SipPOS.Models;
using SipPOS.DataTransfer;

namespace SipPOS.Services.Interfaces;

// Remove this class once your pages/features are using your data.
public interface IProductService
{
    Pagination<ProductDto> Search(IList<object> filters, IList<object> sorts, int perPage, int page);

    IList<ProductDto> GetAll();

    ProductDto? GetById(long id);

    ProductDto? Insert(ProductDto productDto);

    ProductDto? UpdateById(ProductDto productDto);

    ProductDto? DeleteById(long id);

    IList<ProductDto> DeleteByIds(IList<long> ids);

}
