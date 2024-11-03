using SipPOS.DataTransfer;
using SipPOS.Models;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SipPOS.Services;

// Remove this class once your pages/features are using your data.
public interface IProductService
{
    Pagination<ProductDto> Search(ProductFilterDto productFilterDto, SortDto sortDto, int perPage, int page);

    IList<ProductDto> GetAll();

    ProductDto? GetById(long id);

    ProductDto? Insert(ProductDto productDto);

    ProductDto? UpdateById(ProductDto productDto);

    ProductDto? DeleteById(long id);

    IList<ProductDto> DeleteByIds(IList<long> ids);

}
