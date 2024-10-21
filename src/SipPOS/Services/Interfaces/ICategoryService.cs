using SipPOS.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using SipPOS.DataTransfer;

namespace SipPOS.Services;

// Remove this class once your pages/features are using your data.
public interface ICategoryService
{
    IEnumerable<CategoryDto> GetAll();

    CategoryDto GetById(int id);

    CategoryDto Insert(CategoryDto categoryDto);

    CategoryDto UpdateById(CategoryDto categoryDto);

}
