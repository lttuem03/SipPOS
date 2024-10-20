using SipPOS.Models;
using SipPOS.Services;

namespace SipPOS.Services.Impl;

public class ProductService : IProductService
{
    private List<Product> _allProducts = new List<Product>()
    {
        new Product()
        {
            Id = 1,
            CreatedAt = new DateTime(2022, 5, 15),
            UpdatedAt = new DateTime(2022, 6, 10),
            DeletedAt = null,
            Name = "Coca Cola",
            Desc = "Nước giải khát có ga, phổ biến trên toàn thế giới",
            Price = 12.50,
            Status = "Available",
            SymbolCode = 57843,
            SymbolName = "Beverage",
        },
        new Product()
        {
            Id = 2,
            CreatedAt = new DateTime(2021, 8, 22),
            UpdatedAt = new DateTime(2021, 9, 15),
            DeletedAt = null,
            Name = "Pepsi",
            Desc = "Nước ngọt có ga với hương vị truyền thống",
            Price = 11.00,
            Status = "Available",
            SymbolCode = 57844,
            SymbolName = "Beverage",
        },
        new Product()
        {
            Id = 3,
            CreatedAt = new DateTime(2020, 12, 5),
            UpdatedAt = new DateTime(2021, 1, 10),
            DeletedAt = null,
            Name = "Sprite",
            Desc = "Nước giải khát có ga, vị chanh tươi mát",
            Price = 10.75,
            Status = "Unavailable",
            SymbolCode = 57845,
            SymbolName = "Beverage",
        },
        new Product()
        {
            Id = 4,
            CreatedAt = new DateTime(2023, 3, 12),
            UpdatedAt = new DateTime(2023, 4, 5),
            DeletedAt = null,
            Name = "Nước cam ép",
            Desc = "Nước cam ép nguyên chất, giàu vitamin C",
            Price = 15.00,
            Status = "Available",
            SymbolCode = 57846,
            SymbolName = "Fruit",
        },
        new Product()
        {
            Id = 5,
            CreatedAt = new DateTime(2022, 7, 20),
            UpdatedAt = new DateTime(2022, 8, 10),
            DeletedAt = null,
            Name = "Nước ép táo",
            Desc = "Nước ép táo ngọt tự nhiên, giàu chất xơ",
            Price = 14.00,
            Status = "Unavailable",
            SymbolCode = 57847,
            SymbolName = "Fruit",
        }
    };

    public ProductService()
    {
    }

    public async Task<IEnumerable<Product>> Get()
    {
        List<Product> _allProducts = new List<Product>(this._allProducts);
        await Task.CompletedTask;
        return _allProducts;
    }

    public async Task<Product> Get(int id)
    {
        List<Product> _allProducts = new List<Product>(this._allProducts);
        await Task.CompletedTask;
        return _allProducts.Find(x => x.Id == id);
    }

    public async Task<Product> Add(Product product)
    {
        _allProducts.Add(product);
        await Task.CompletedTask;
        return product;
    }

    public async Task<Product> Update(Product product)
    {
        _allProducts[_allProducts.FindIndex(x => x.Id == product.Id)] = product;
        return await Task.FromResult(product);
    }

    public async Task<Product> Delete(int id)
    {
        _allProducts[_allProducts.FindIndex(x => x.Id == id)].DeletedAt = DateTime.Now;
        return await Task.FromResult(_allProducts[_allProducts.FindIndex(x => x.Id == id)]);
    }

    public async Task<Product> Delete(Product product)
    {
        _allProducts[_allProducts.FindIndex(x => x.Id == product.Id)].DeletedAt = DateTime.Now;
        return await Task.FromResult(_allProducts[_allProducts.FindIndex(x => x.Id == product.Id)]);
    }

    public async Task<IEnumerable<Product>> GetByCategory(int categoryId)
    {
        List<Product> products = new List<Product>();
        foreach (var product in _allProducts)
        {
            if (product.CategoryId == categoryId)
            {
                products.Add(product);
            }
        }
        return await Task.FromResult(products);
    }
}
