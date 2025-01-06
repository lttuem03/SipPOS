using SipPOS.Models.Entity;
using SipPOS.Models.General;
using SipPOS.DataTransfer.General;
using SipPOS.Services.DataAccess.Interfaces;

namespace SipPOS.Services.DataAccess.Implementations;

/// <summary>
/// Mock implementation of the IProductDao interface for testing purposes.
/// </summary>
public class MockProductDao : IProductDao
{
    /// <summary>
    /// List of mock Product.
    /// </summary>
    private readonly List<Product> _allProducts = new()
    {
        // 0. Phin Sữa Đá (danh mục 0. Cà phê PHIN)
        new()
        {
            Id = 0,
            StoreId = 0,
            CreatedBy = "admin",
            CreatedAt = DateTime.Now,
            Name = "Phin Sữa Đá",
            Description = "Iced Coffee with Condensed Milk",
            ProductOptions = new()
            {
                ("S", 29_000m), ("M", 39_000m), ("L", 45_000m)
            },
            CategoryId = 0,
            ImageUris = new List<string>
            {
                ""
            },
            Status = "Available"
        },
        // 1. Phin Đen Đá (danh mục 0. Cà phê PHIN)
        new()
        {
            Id = 1,
            StoreId = 0,
            CreatedBy = "admin",
            CreatedAt = DateTime.Now,
            Name = "Phin Đen Đá",
            Description = "Iced Black Coffee",
            ProductOptions = new()
            {
                ("S", 29_000m), ("M", 35_000m), ("L", 39_000m)
            },
            CategoryId = 0,
            ImageUris = new List<string>
            {
                ""
            },
            Status = "Available"
        },
        // 2. Bạc Xỉu (danh mục 0. Cà phê PHIN)
        new()
        {
            Id = 2,
            StoreId = 0,
            CreatedBy = "admin",
            CreatedAt = DateTime.Now,
            Name = "Bạc Xỉu",
            Description = "Iced White PHIN Coffee & Condensed Milk",
            ProductOptions = new()
            {
                ("S", 29_000m), ("M", 39_000m), ("L", 45_000m)
            },
            CategoryId = 0,
            ImageUris = new List<string>
            {
                "https://www.highlandscoffee.com.vn/vnt_upload/product/04_2023/New_product/HLC_New_logo_5.1_Products__BAC_XIU.jpg"
            },
            Status = "Available"
        },
        // 3. PhinDi Hạnh Nhân (danh mục 1. PhinDi)
        new()
        {
            Id = 3,
            StoreId = 0,
            CreatedBy = "admin",
            CreatedAt = DateTime.Now,
            Name = "PhinDi Hạnh Nhân",
            Description = "Iced Coffee with Almond & Fresh Milk",
            ProductOptions = new()
            {
                ("S", 45_000m), ("M", 49_000m), ("L", 55_000m)
            },
            CategoryId = 1,
            ImageUris = new List<string>
            {
                "https://www.highlandscoffee.com.vn/vnt_upload/product/06_2023/HLC_New_logo_5.1_Products__PHINDI_HANH_NHAN.jpg"
            },
            Status = "Available"
        },
        // 4. PhinDi Kem Sữa (danh mục 1. PhinDi)
        new()
        {
            Id = 4,
            StoreId = 0,
            CreatedBy = "admin",
            CreatedAt = DateTime.Now,
            Name = "PhinDi Kem Sữa",
            Description = "Iced Coffee with Milk Foam",
            ProductOptions = new()
            {
                ("S", 45_000m), ("M", 49_000m), ("L", 55_000m)
            },
            CategoryId = 1,
            ImageUris = new List<string>
            {
                "https://www.highlandscoffee.com.vn/vnt_upload/product/06_2023/HLC_New_logo_5.1_Products__PHINDI_KEM_SUA.jpg"
            },
            Status = "Available"
        },
        // 5. PhinDi Choco (danh mục 1. PhinDi)
        new()
        {
            Id = 5,
            StoreId = 0,
            CreatedBy = "admin",
            CreatedAt = DateTime.Now,
            Name = "PhinDi Choco",
            Description = "Iced Coffee with Chocolate",
            ProductOptions = new()
            {
                ("S", 45_000m), ("M", 49_000m), ("L", 55_000m)
            },
            CategoryId = 1,
            ImageUris = new List<string>
            {
                "https://www.highlandscoffee.com.vn/vnt_upload/product/06_2023/HLC_New_logo_5.1_Products__PHINDI_CHOCO.jpg"
            },
            Status = "Available"
        },
        // 6. Trà Sen Vàng (danh mục 2. Trà)
        new()
        {
            Id = 6,
            StoreId = 0,
            CreatedBy = "admin",
            CreatedAt = DateTime.Now,
            Name = "Trà Sen Vàng",
            Description = "Tea with Lotus Seeds",
            ProductOptions = new()
            {
                ("S", 45_000m), ("M", 55_000m), ("L", 65_000m)
            },
            CategoryId = 2,
            ImageUris = new List<string>
            {
                "https://www.highlandscoffee.com.vn/vnt_upload/product/06_2023/HLC_New_logo_5.1_Products__TSV.jpg"
            },
            Status = "Available"
        },
        // 7. Trà Thạch Đào (danh mục 2. Trà)
        new()
        {
            Id = 7,
            StoreId = 0,
            CreatedBy = "admin",
            CreatedAt = DateTime.Now,
            Name = "Trà Thạch Đào",
            Description = "Tea with Peach Jelly",
            ProductOptions = new()
            {
                ("S", 45_000m), ("M", 55_000m), ("L", 65_000m)
            },
            CategoryId = 2,
            ImageUris = new List<string>
            {
                "https://www.highlandscoffee.com.vn/vnt_upload/product/06_2023/HLC_New_logo_5.1_Products__TRA_THANH_DAO-09.jpg"
            },
            Status = "Available"
        },
        // 8. Trà Thanh Đào (danh mục 2. Trà)
        new()
        {
            Id = 8,
            StoreId = 0,
            CreatedBy = "admin",
            CreatedAt = DateTime.Now,
            Name = "Trà Thanh Đào",
            Description = "Peach Tea with Lemongrass",
            ProductOptions = new()
            {
                ("S", 45_000m), ("M", 55_000m), ("L", 65_000m)
            },
            CategoryId = 2,
            ImageUris = new List<string>
            {
                "https://www.highlandscoffee.com.vn/vnt_upload/product/06_2023/HLC_New_logo_5.1_Products__TRA_THANH_DAO-08.jpg"
            },
            Status = "Available"
        },
        // 9. Trà Thạch Vải (danh mục 2. Trà)
        new()
        {
            Id = 9,
            StoreId = 0,
            CreatedBy = "admin",
            CreatedAt = DateTime.Now,
            Name = "Trà Thạch Vải",
            Description = "Tea with Lychee Jelly",
            ProductOptions = new()
            {
                ("S", 45_000m), ("M", 55_000m), ("L", 65_000m)
            },
            CategoryId = 2,
            ImageUris = new List<string>
            {
                "https://www.highlandscoffee.com.vn/vnt_upload/product/06_2023/HLC_New_logo_5.1_Products__TRA_TACH_VAI.jpg"
            },
            Status = "Available"
        },
        // 10. Trà Xanh Đậu Đỏ (danh mục 2. Trà)
        new()
        {
            Id = 10,
            StoreId = 0,
            CreatedBy = "admin",
            CreatedAt = DateTime.Now,
            Name = "Trà Xanh Đậu Đỏ",
            Description = "Green Tea with Red Bean",
            ProductOptions = new()
            {
                ("S", 45_000m), ("M", 55_000m), ("L", 65_000m)
            },
            CategoryId = 2,
            ImageUris = new List<string>
            {
                "https://www.highlandscoffee.com.vn/vnt_upload/product/06_2023/HLC_New_logo_5.1_Products__TRA_XANH_DAU_DO.jpg"
            },
            Status = "Available"
        },
        // 11. Freeze Trà Xanh (danh mục 3. Freeze)
        new()
        {
            Id = 11,
            StoreId = 0,
            CreatedBy = "admin",
            CreatedAt = DateTime.Now,
            Name = "Freeze Trà Xanh",
            Description = "Green Tea Freeze",
            ProductOptions = new()
            {
                ("S", 55_000m), ("M", 65_000m), ("L", 69_000m)
            },
            CategoryId = 3,
            ImageUris = new List<string>
            {
                "https://www.highlandscoffee.com.vn/vnt_upload/product/06_2023/HLC_New_logo_5.1_Products__FREEZE_TRA_XANH.jpg"
            },
            Status = "Available"
        },
        // 12. Caramel Phin Freeze (danh mục 3. Freeze)
        new()
        {
            Id = 12,
            StoreId = 0,
            CreatedBy = "admin",
            CreatedAt = DateTime.Now,
            Name = "Caramel Phin Freeze",
            Description = "Caramel Phin Freeze",
            ProductOptions = new()
            {
                ("S", 55_000m), ("M", 65_000m), ("L", 69_000m)
            },
            CategoryId = 3,
            ImageUris = new List<string>
            {
                "https://www.highlandscoffee.com.vn/vnt_upload/product/06_2023/HLC_New_logo_5.1_Products__CARAMEL_FREEZE_PHINDI.jpg"
            },
            Status = "Available"
        },
        // 13. Cookies & Cream (danh mục 3. Freeze)
        new()
        {
            Id = 13,
            StoreId = 0,
            CreatedBy = "admin",
            CreatedAt = DateTime.Now,
            Name = "Cookies & Cream",
            Description = "Cookies & Cream",
            ProductOptions = new()
            {
                ("S", 55_000m), ("M", 65_000m), ("L", 69_000m)
            },
            CategoryId = 3,
            ImageUris = new List<string>
            {
                "https://www.highlandscoffee.com.vn/vnt_upload/product/06_2023/HLC_New_logo_5.1_Products__COOKIES_FREEZE.jpg"
            },
            Status = "Available"
        },
        // 14. Freeze Sô-cô-la (danh mục 3. Freeze)
        new()
        {
            Id = 14,
            StoreId = 0,
            CreatedBy = "admin",
            CreatedAt = DateTime.Now,
            Name = "Freeze Sô-cô-la",
            Description = "Chocolate Freeze",
            ProductOptions = new()
            {
                ("S", 55_000m), ("M", 65_000m), ("L", 69_000m)
            },
            CategoryId = 3,
            ImageUris = new List<string>
            {
                "https://www.highlandscoffee.com.vn/vnt_upload/product/04_2023/New_product/HLC_New_logo_5.1_Products__FREEZE_CHOCO.jpg"
            },
            Status = "Available"
        },
        // 15. Classic Phin Freeze (danh mục 3. Freeze)
        new()
        {
            Id = 15,
            StoreId = 0,
            CreatedBy = "admin",
            CreatedAt = DateTime.Now,
            Name = "Classic Phin Freeze",
            Description = "Classic Phin Freeze",
            ProductOptions = new()
            {
                ("S", 55_000m), ("M", 65_000m), ("L", 69_000m)
            },
            CategoryId = 3,
            ImageUris = new List<string>
            {
                "https://www.highlandscoffee.com.vn/vnt_upload/product/06_2023/HLC_New_logo_5.1_Products__CLASSIC_FREEZE_PHINDI.jpg"
            },
            Status = "Available"
        },
        // 16. Bánh Croissant (danh mục 4. Bánh)
        new()
        {
            Id = 16,
            StoreId = 0,
            CreatedBy = "admin",
            CreatedAt = DateTime.Now,
            Name = "Bánh Croissant",
            Description = "Quaso",
            ProductOptions = new()
            {
                ("S", 29_000m)
            },
            CategoryId = 4,
            ImageUris = new List<string>
            {
                "https://www.highlandscoffee.com.vn/vnt_upload/product/11_2024/2024_Food/Croissant.png"
            },
            Status = "Available"
        },
        // 17. Bánh Chuối (danh mục 4. Bánh)
        new()
        {
            Id = 17,
            StoreId = 0,
            CreatedBy = "admin",
            CreatedAt = DateTime.Now,
            Name = "Bánh Chuối",
            Description = "Banana pie",
            ProductOptions = new()
            {
                ("S", 29_000m)
            },
            CategoryId = 4,
            ImageUris = new List<string>
            {
                "https://www.highlandscoffee.com.vn/vnt_upload/product/11_2024/thumbs/Banh-Chuoi.png"
            },
            Status = "Available"
        },
        // 18. Bánh Phô mai Cà phê (danh mục 4. Bánh)
        new()
        {
            Id = 18,
            StoreId = 0,
            CreatedBy = "admin",
            CreatedAt = DateTime.Now,
            Name = "Bánh Phô mai Cà phê",
            Description = "Coffee-flavour Cheesecake",
            ProductOptions = new()
            {
                ("S", 29_000m)
            },
            CategoryId = 4,
            ImageUris = new List<string>
            {
                "https://www.highlandscoffee.com.vn/vnt_upload/product/03_2018/PHOMAICAPHE.jpg"
            },
            Status = "Available"
        },
        // 19. Bánh Phô mai Chanh dây (danh mục 4. Bánh)
        new()
        {
            Id = 19,
            StoreId = 0,
            CreatedBy = "admin",
            CreatedAt = DateTime.Now,
            Name = "Bánh Phô mai Chanh dây",
            Description = "Passion-fruit-flavour Cheesecake",
            ProductOptions = new()
            {
                ("S", 29_000m)
            },
            CategoryId = 4,
            ImageUris = new List<string>
            {
                "https://www.highlandscoffee.com.vn/vnt_upload/product/03_2018/PHOMAICHANHDAY.jpg"
            },
            Status = "Available"
        },
        // 20. Bánh Phô mai Trà Xanh (danh mục 4. Bánh)
        new()
        {
            Id = 20,
            StoreId = 0,
            CreatedBy = "admin",
            CreatedAt = DateTime.Now,
            Name = "Bánh Phô mai Trà Xanh",
            Description = "Green tea flavour Cheesecake",
            ProductOptions = new()
            {
                ("S", 35_000m)
            },
            CategoryId = 4,
            ImageUris = new List<string>
            {
                "https://www.highlandscoffee.com.vn/vnt_upload/product/11_2024/thumbs/Pho-Mai-Tra-Xanh.png"
            },
            Status = "Available"
        },
        // 21. Bánh Caramel Phô mai (danh mục 4. Bánh)
        new()
        {
            Id = 21,
            StoreId = 0,
            CreatedBy = "admin",
            CreatedAt = DateTime.Now,
            Name = "Bánh Caramel Phô mai",
            Description = "Caramel cheesecake",
            ProductOptions = new()
            {
                ("S", 35_000m)
            },
            CategoryId = 4,
            ImageUris = new List<string>
            {
                "https://www.highlandscoffee.com.vn/vnt_upload/product/03_2018/CARAMELPHOMAI.jpg"
            },
            Status = "Available"
        },
        // 22. Bánh Tiramisu (danh mục 4. Bánh)
        new()
        {
            Id = 22,
            StoreId = 0,
            CreatedBy = "admin",
            CreatedAt = DateTime.Now,
            Name = "Bánh Tiramisu",
            Description = "Tiramisu cake",
            ProductOptions = new()
            {
                ("S", 35_000m)
            },
            CategoryId = 4,
            ImageUris = new List<string>
            {
                "https://www.highlandscoffee.com.vn/vnt_upload/product/11_2024/thumbs/Tiramisu.png"
            },
            Status = "Available"
        },
        // 23. Bánh Mousse Đào (danh mục 4. Bánh)
        new()
        {
            Id = 23,
            StoreId = 0,
            CreatedBy = "admin",
            CreatedAt = DateTime.Now,
            Name = "Bánh Mousse Đào",
            Description = "Peach mousse",
            ProductOptions = new()
            {
                ("S", 35_000m)
            },
            CategoryId = 4,
            ImageUris = new List<string>
            {
                "https://www.highlandscoffee.com.vn/vnt_upload/product/11_2024/thumbs/Mousse-Dao.png"
            },
            Status = "Available"
        },
        // 24. Bánh Mousse Cacao (danh mục 4. Bánh)
        new()
        {
            Id = 24,
            StoreId = 0,
            CreatedBy = "admin",
            CreatedAt = DateTime.Now,
            Name = "Bánh Mousse Cacao",
            Description = "Cacao mousse",
            ProductOptions = new()
            {
                ("S", 35_000m)
            },
            CategoryId = 4,
            ImageUris = new List<string>
            {
                "https://www.highlandscoffee.com.vn/vnt_upload/product/11_2024/thumbs/Mousse-Cacao.png"
            },
            Status = "Available"
        }
    };

    /// <summary>
    /// Inserts a new product.
    /// </summary>
    /// <param name="product">The product to insert.</param>
    /// <returns>The inserted product.</returns>
    public Task<Product?> InsertAsync(long storeId, Product product)
    {
        product.Id = new DateTime().Ticks;
        product.StoreId = storeId;
        product.CreatedAt = DateTime.Now;
        product.CreatedBy = "admin";

        _allProducts.Add(product);

        Product? result = new Product();

        result.Id = product.Id;
        result.StoreId = product.StoreId;
        result.CreatedAt = product.CreatedAt;
        result.CreatedBy = product.CreatedBy;
        result.Name = product.Name;
        result.Description = product.Description;
        result.ProductOptions = product.ProductOptions;
        result.ItemsSold = product.ItemsSold;
        result.CategoryId = product.CategoryId;
        result.ImageUris = product.ImageUris;
        result.Status = product.Status;

        return Task.FromResult((Product?)result);
    }

    /// <summary>
    /// Retrieves all products that are not marked as deleted.
    /// </summary>
    /// <returns>A list of all available products.</returns>
    public Task<List<Product>> GetAllAsync(long storeId)
    {
        return Task.FromResult(_allProducts.Where(_allProducts => _allProducts.DeletedAt == null).ToList());
    }

    /// <summary>
    /// Retrieves a product by its ID.
    /// </summary>
    /// <param name="id">The ID of the product to retrieve.</param>
    /// <returns>The product if found; otherwise, null.</returns>
    public Task<Product?> GetByIdAsync(long storeId, long id)
    {
        return Task.FromResult((Product?)_allProducts.First(x => x.Id == id));
    }

    /// <summary>
    /// Perform a search for products in a pagination.
    /// </summary>
    /// <param name="storeId">The store ID of the store to retrieve from.</param>
    /// <param name="productFilterDto">The filters to apply.</param>
    /// <param name="sortDto">The sorting options to apply.</param>
    /// <param name="page">The page number to retrieve.</param>
    /// 
    /// <param name="perPage">The number of products per page.</param>
    /// <returns>A pagination object containing the search results.</returns>
    public Task<Pagination<Product>> GetWithPaginationAsync
    (
        long storeId,
        ProductFilterDto productFilterDto, 
        SortDto sortDto,
        int page, 
        int perPage
    )
    {
        var _allProducts = GetAllAsync(storeId).Result;
        var pagination = new Pagination<Product>();

        var filteredProducts = _allProducts.AsQueryable();

        if (!string.IsNullOrEmpty(productFilterDto.Name))
        {
            filteredProducts = filteredProducts.Where(x => x.Name != null && x.Name.Contains(productFilterDto.Name));
        }

        if (!string.IsNullOrEmpty(productFilterDto.Desc))
        {
            filteredProducts = filteredProducts.Where(x => x.Description != null && x.Description.Contains(productFilterDto.Desc));
        }

        if (productFilterDto.CategoryId.HasValue)
        {
            filteredProducts = filteredProducts.Where(x => x.CategoryId == productFilterDto.CategoryId.Value);
        }

        if (!string.IsNullOrEmpty(productFilterDto.Status))
        {
            filteredProducts = filteredProducts.Where(x => x.Status != null && x.Status == productFilterDto.Status);
        }

        Func<Product, object> keySelector = x => sortDto.SortBy switch
        {
            "Id" => x.Id,
            "Name" => x.Name ?? string.Empty,
            "Desc" => x.Description ?? string.Empty,
            //"Price" => x.Price ?? 0,
            "Category" => x.CategoryId ?? 0,
            "Status" => x.Status ?? string.Empty,
            "CreatedBy" => x.CreatedBy ?? string.Empty,
            _ => x.CreatedAt ?? new DateTime()
        };

        pagination.Data = (sortDto.SortType == "DESC"
            ? filteredProducts.OrderByDescending(keySelector)
            : filteredProducts.OrderBy(keySelector))
            .Skip((page - 1) * perPage)
            .Take(perPage)
            .ToList();

        pagination.Page = page;
        pagination.PerPage = perPage;
        pagination.TotalRecord = filteredProducts.Count();
        pagination.TotalPage = (int)Math.Ceiling((double)filteredProducts.Count() / perPage);

        return Task.FromResult(pagination);
    }

    /// <summary>
    /// Updates an existing product by its ID.
    /// </summary>
    /// <param name="product">The product with updated information.</param>
    /// <returns>The updated product if found; otherwise, null.</returns>
    public Task<Product?> UpdateByIdAsync(long storeId, Product product)
    {
        var oldProduct = GetByIdAsync(storeId, product.Id).Result;

        if (oldProduct != null)
        {
            oldProduct.Name = product.Name;
            oldProduct.Description = product.Description;
            oldProduct.ProductOptions = product.ProductOptions;
            oldProduct.CategoryId = product.CategoryId;
            oldProduct.Status = product.Status;
            oldProduct.UpdatedAt = DateTime.Now;
            oldProduct.UpdatedBy = "admin";
        }

        return Task.FromResult(oldProduct);
    }

    /// <summary>
    /// "Soft" deletes a product by its ID.
    /// </summary>
    /// <param name="storeId">The store ID of the store to delete product from.</param>
    /// <param name="id">The ID of the product to delete.</param>
    /// <returns>The deleted product if successful; otherwise, null.</returns>
    public Task<Product?> DeleteByIdAsync(long storeId, long id, Staff author)
    {
        var product = GetByIdAsync(storeId, id).Result;

        if (product != null)
        {
            //do for soft delete
            product.DeletedAt = DateTime.Now;
            product.DeletedBy = "admin";
        }

        return Task.FromResult(product);
    }

    /// <summary>
    /// Deletes multiple products by their IDs.
    /// </summary>
    /// <param name="ids">The list of IDs of the products to delete.</param>
    /// <returns>A list of deleted products.</returns>
    public Task<List<Product>> DeleteByIdsAsync(long storeId, List<long> ids, Staff author)
    {
        var products = new List<Product>();

        foreach (var id in ids)
        {
            var deletedProduct = DeleteByIdAsync(storeId, id, author).Result;

            if (deletedProduct != null)
            {
                products.Add(deletedProduct);
            }
        }

        return Task.FromResult(products);
    }

    /// <summary>
    /// Counts the total number of products.
    /// </summary>
    /// <returns>The total number of products.</returns>
    public Task<long> CountAsync(long storeId, ProductFilterDto productFilterDto)
    {
        return Task.FromResult((long)_allProducts.Count);
    }
}