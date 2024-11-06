using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using Windows.Data.Pdf;

using SipPOS.Models;
using SipPOS.DataAccess.Interfaces;

namespace SipPOS.DataAccess.Implementations;

public class MockProductDao : IProductDao
{
    private readonly List<Product> _allProducts = new()
    {
        new()
        {
            Id = 1,
            CreatedAt = new DateTime(2022, 5, 15),
            CreatedBy = "admin",
            Name = "Coca Cola",
            ImageUrls = new List<string> {
                "https://www.austriajuice.com/hs-fs/hubfs/Beverage_compounds_drinks.jpg?width=730&name=Beverage_compounds_drinks.jpg",
                "https://s7d1.scene7.com/is/image/KeminIndustries/shutterstock_519547867?$responsive$"
            },
            Desc = "Nước giải khát có ga, phổ biến trên toàn thế giới",
            Price = 12.50,
            CategoryId = 1,
            Status = "Available",
        },
        new()
        {
            Id = 2,
            CreatedAt = new DateTime(2021, 8, 22),
            CreatedBy = "admin",
            Name = "Pepsi",
            Desc = "Nước ngọt có ga với hương vị truyền thống",
            Price = 11.00,
            CategoryId = 1,
            Status = "Available",
        },
        new()
        {
            Id = 3,
            CreatedAt = new DateTime(2020, 12, 5),
            CreatedBy = "admin",
            Name = "Sprite",
            ImageUrls = new List<string> {
                "https://www.austriajuice.com/hs-fs/hubfs/Beverage_compounds_drinks.jpg?width=730&name=Beverage_compounds_drinks.jpg",
                "https://s7d1.scene7.com/is/image/KeminIndustries/shutterstock_519547867?$responsive$"
            },
            Desc = "Nước giải khát có ga, vị chanh tươi mát",
            Price = 10.75,
            CategoryId = 1,
            Status = "Unavailable",
        },
        new()
        {
            Id = 4,
            CreatedAt = new DateTime(2023, 3, 12),
            CreatedBy = "admin",
            Name = "Nước cam ép",
            Desc = "Nước cam ép nguyên chất, giàu vitamin C",
            Price = 15.00,
            CategoryId = 2,
            Status = "Available",
        },
        new()
        {
            Id = 5,
            CreatedAt = new DateTime(2022, 7, 20),
            CreatedBy = "admin",
            Name = "Nước ép táo",
            Desc = "Nước ép táo ngọt tự nhiên, giàu chất xơ",
            Price = 14.00,
            CategoryId = 2,
            Status = "Unavailable",
        },
        new()
        {
            Id = 6,
            CreatedAt = new DateTime(2021, 10, 15),
            CreatedBy = "admin",
            Name = "Nước ép dừa",
            Desc = "Nước ép dừa tươi ngon, giàu chất khoáng",
            Price = 16.00,
            CategoryId = 2,
            Status = "Available",
        },
        new()
        {
            Id = 7,
            CreatedAt = new DateTime(2020, 11, 5),
            CreatedBy = "admin",
            Name = "Nước suối Lavie",
            Desc = "Nước suối Lavie nguyên chất, không chất bảo quản",
            Price = 5.00,
            CategoryId = 3,
            Status = "Available",
        },
        new()
        {
            Id = 8,
            CreatedAt = new DateTime(2023, 3, 12),
            CreatedBy = "admin",
            Name = "Nước suối Aquafina",
            Desc = "Nước suối Aquafina nguyên chất, không chất bảo quản",
            Price = 6.00,
            CategoryId = 3,
            Status = "Available",
        },
        new()
        {
            Id = 9,
            CreatedAt = new DateTime(2022, 7, 20),
            CreatedBy = "admin",
            Name = "Nước suối Dasani",
            Desc = "Nước suối Dasani nguyên chất, không chất bảo quản",
            Price = 7.00,
            CategoryId = 3,
            Status = "Unavailable",
        },
        new()
        {
            Id = 10,
            CreatedAt = new DateTime(2021, 10, 15),
            CreatedBy = "admin",
            Name = "Red Bull",
            Desc = "Nước tăng lực Red Bull, giúp tăng cường năng lượng",
            Price = 8.00,
            CategoryId = 5,
            Status = "Available",
        },
        new()
        {
            Id = 11,
            CreatedAt = new DateTime(2020, 11, 5),
            CreatedBy = "admin",
            Name = "Sting",
            Desc = "Nước tăng lực Sting, giúp tăng cường năng lượng",
            Price = 9.00,
            CategoryId = 5,
            Status = "Available",
        },
    };

    /// <summary>
    /// Deletes a product by its ID.
    /// </summary>
    /// <param name="id">The ID of the product to delete.</param>
    /// <returns>The deleted product if found; otherwise, null.</returns>
    public Product? DeleteById(long id)
    {
        Product product = GetById(id);
        if (product != null)
        {
            //do for soft delete
            product.DeletedAt = DateTime.Now;
            product.DeletedBy = "admin";
        }
        return product;
    }

    /// <summary>
    /// Deletes multiple products by their IDs.
    /// </summary>
    /// <param name="ids">The list of IDs of the products to delete.</param>
    /// <returns>A list of deleted products.</returns>
    public IList<Product> DeleteByIds(IList<long> ids)
    {
        List<Product> products = new List<Product>();

        foreach (var id in ids)
        {
            Product? deletedProduct = DeleteById(id);

            if (deletedProduct != null)
            {
                products.Add(deletedProduct);
            }
        }

        return products;
    }

    /// <summary>
    /// Retrieves all products that are not marked as deleted.
    /// </summary>
    /// <returns>A list of all available products.</returns>
    public IList<Product> GetAll()
    {
        return _allProducts.Where(_allProducts => _allProducts.DeletedAt == null).ToList();
    }

    /// <summary>
    /// Retrieves a product by its ID.
    /// </summary>
    /// <param name="id">The ID of the product to retrieve.</param>
    /// <returns>The product if found; otherwise, null.</returns>
    public Product GetById(long id)
    {
        return _allProducts.First(x => x.Id == id);
    }

    /// <summary>
    /// Inserts a new product.
    /// </summary>
    /// <param name="product">The product to insert.</param>
    /// <returns>The inserted product.</returns>
    public Product? Insert(Product product)
    {
        product.Id = new DateTime().Ticks;
        product.CreatedAt = DateTime.Now;
        product.CreatedBy = "admin";
        _allProducts.Add(product);
        return product;
    }

    /// <summary>
    /// Updates an existing product by its ID.
    /// </summary>
    /// <param name="product">The product with updated information.</param>
    /// <returns>The updated product if found; otherwise, null.</returns>
    public Product? UpdateById(Product product)
    {
        Product oldProduct = GetById(product.Id);

        if (oldProduct != null)
        {
            oldProduct.Name = product.Name;
            oldProduct.Desc = product.Desc;
            oldProduct.Price = product.Price;
            oldProduct.CategoryId = product.CategoryId;
            oldProduct.Status = product.Status;
            oldProduct.UpdatedAt = DateTime.Now;
            oldProduct.UpdatedBy = "admin";
        }
        return oldProduct;
    }

    /// <summary>
    /// Counts the total number of products.
    /// </summary>
    /// <returns>The total number of products.</returns>
    public long Count()
    {
        return _allProducts.Count;
    }

    /// <summary>
    /// Perform a search for products in a pagination.
    /// </summary>
    /// <param name="filters">The filters to apply.</param>
    /// <param name="sorts">The sorting options to apply.</param>
    /// <param name="page">The page number to retrieve.</param>
    /// <param name="perPage">The number of products per page.</param>
    /// <returns>A pagination object containing the search results.</returns>
    public Pagination<Product> Search(IList<object> filters, IList<object> sorts, int page,  int perPage)
    {
        IList<Product> _allProducts = GetAll();
        Pagination<Product> pagination = new Pagination<Product>();
        pagination.Data = _allProducts.OrderByDescending(x => x.CreatedAt)
            .Skip((page - 1) * perPage).Take(perPage).ToList();
        pagination.Page = page;
        pagination.PerPage = perPage;
        pagination.TotalRecord = _allProducts.Count;
        pagination.TotalPage = (int)Math.Ceiling((double)_allProducts.Count / perPage);
        return pagination;
    }
}
