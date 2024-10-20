using SipPOS.Models;
using SipPOS.Services;

namespace SipPOS.Services.Impl;

public class CategoryService : ICategoryService
{
    private List<Category> _allCategory = new List<Category>()
    {

        new Category()
        {
            Id = 1,
            Name = "Nước giải khát",
            Desc = "Đây là danh mục nước giải khát",
        },
        new Category()
        {
            Id = 2,
            Name = "Nước trái cây",
            Desc = "Danh mục các loại nước ép trái cây",
            Products = new List<Product>()
        }
    };

    public CategoryService()
    {

    }

    public async Task<IEnumerable<Category>> Get()
    {
        List<Category> _allCategory = new List<Category>(this._allCategory);
        await Task.CompletedTask;
        return _allCategory;
    }

    public async Task<Category?> Get(int id)
    {
        List<Category> _allCategory = new List<Category>(this._allCategory);
        await Task.CompletedTask;
        return _allCategory.Find(x => x.Id == id);
    }

    public async Task<Category> Add(Category product)
    {
        _allCategory.Add(product);
        await Task.CompletedTask;
        return product;
    }

    public async Task<Category> Update(Category product)
    {
        _allCategory[_allCategory.FindIndex(x => x.Id == product.Id)] = product;
        return await Task.FromResult(product);
    }
}
