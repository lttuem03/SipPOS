using System.Collections.Generic;
using System.Threading.Tasks;

using SipPOS.Models;
using SipPOS.DataTransfer;
using SipPOS.DataAccess.Interfaces;

namespace SipPOS.Services.Interfaces;

// Remove this class once your pages/features are using your data.
public interface ICategoryService
{
    Pagination<CategoryDto> Search(IList<object> filters, IList<object> sorts, int perPage, int page);

    IList<CategoryDto> GetAll();

    CategoryDto? GetById(long id);
    
    CategoryDto? DeleteById(long id);
    
    IList<CategoryDto> DeleteByIds(IList<long> ids);
    
    CategoryDto? Insert(CategoryDto categoryDto);

    CategoryDto? UpdateById(CategoryDto categoryDto);

}
