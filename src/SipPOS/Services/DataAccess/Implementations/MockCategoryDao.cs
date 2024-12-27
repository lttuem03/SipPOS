using SipPOS.DataTransfer.General;
using SipPOS.Models.Entity;
using SipPOS.Models.General;
using SipPOS.Services.DataAccess.Interfaces;

namespace SipPOS.Services.DataAccess.Implementations;

/// <summary>
/// Mock implementation of the ICategoryDao interface for testing purposes.
/// Providing mock data for a few Category objects.
/// </summary>
public class MockCategoryDao : ICategoryDao
{
    private readonly List<Category> _allCategory =
    [
        // 0. Cà phê PHIN
        new()
        {
            Id = 0,
            StoreId = 0,
            CreatedBy = "admin",
            CreatedAt = DateTime.Now,
            Name = "Cà phê PHIN",
            Description = "Sự kết hợp hoàn hảo giữa hạt cà phê Robusta & Arabica thượng hạng được trồng trên những vùng cao nguyên Việt Nam.",
        },
        // 1. PhinDi
        new()
        {
            Id = 1,
            StoreId = 0,
            CreatedBy = "admin",
            CreatedAt = DateTime.Now,
            Name = "PhinDi",
            //Description = "",
        },
        // 2. Trà
        new()
        {
            Id = 2,
            CreatedBy = "admin",
            CreatedAt = DateTime.Now,
            Name = "Trà",
            Description = "Hương vị tự nhiên, thơm ngon của Trà Việt với phong cách hiện đại sẽ giúp bạn gợi mở vị giác của bản thân và tận hưởng một cảm giác thật khoan khoái, tươi mới.",
        },
        // 3. Freeze
        new()
        {
            Id = 3,
            CreatedBy = "admin",
            CreatedAt = DateTime.Now,
            Name = "Freeze",
            Description = "Thức uống đá xay mát lạnh được pha chế từ những nguyên liệu thuần túy của Việt Nam.",
        },
        // 4. Bánh
        new()
        {
            Id = 3,
            CreatedBy = "admin",
            CreatedAt = DateTime.Now,
            Name = "Bánh",
            Description = "Sẽ càng ngon miệng hơn khi bạn kết hợp đồ uống với những chiếc bánh ngọt thơm ngon được làm thủ công hàng ngày ngay tại bếp bánh của chúng tôi.",
        }
    ];

    /// <summary>
    /// Deletes a category by its ID.
    /// </summary>
    /// <param name="id">The ID of the category to delete.</param>
    /// <returns>The deleted category if found; otherwise, null.</returns>
    public Category? DeleteById(long id)
    {
        var category = GetById(id);

        if (category != null)
        {
            _allCategory.Remove(category);
        }

        return category;
    }

    /// <summary>
    /// Deletes multiple categories by their IDs.
    /// </summary>
    /// <param name="ids">The list of IDs of the categories to delete.</param>
    /// <returns>A list of deleted categories.</returns>
    public IList<Category> DeleteByIds(IList<long> ids)
    {
        var products = new List<Category>();

        foreach (var id in ids)
        {
            var deletedCategory = DeleteById(id);

            if (deletedCategory != null)
            {
                products.Add(deletedCategory);
            }
        }

        return products;
    }

    /// <summary>
    /// Retrieves all categories that are not marked as deleted.
    /// </summary>
    /// <returns>A list of all available categories.</returns>
    public IList<Category> GetAll()
    {
        return _allCategory.Where(x => x.DeletedAt == null).ToList();
    }

    /// <summary>
    /// Retrieves a category by its ID.
    /// </summary>
    /// <param name="id">The ID of the category to retrieve.</param>
    /// <returns>The category if found; otherwise, null.</returns>
    public Category? GetById(long id)
    {
        return _allCategory.FirstOrDefault(x => x.Id == id);
    }

    /// <summary>
    /// Inserts a new category.
    /// </summary>
    /// <param name="category">The category to insert.</param>
    /// <returns>The inserted category.</returns>
    public Category? Insert(Category category)
    {
        _allCategory.Add(category);
        category.CreatedAt = DateTime.Now;
        category.CreatedBy = "admin";
        return category;
    }

    /// <summary>
    /// Updates an existing category by its ID.
    /// </summary>
    /// <param name="category">The category with updated information.</param>
    /// <returns>The updated category if found; otherwise, null.</returns>
    public Category? UpdateById(Category category)
    {
        var oldCategory = GetById(category.Id);

        if (oldCategory != null)
        {
            oldCategory.Name = category.Name;
            oldCategory.Description = category.Description;
            oldCategory.UpdatedAt = DateTime.Now;
            oldCategory.UpdatedBy = "admin";
        }

        return oldCategory;
    }

    /// <summary>
    /// Counts the total number of categories.
    /// </summary>
    /// <returns>The total number of categories.</returns>
    public long Count()
    {
        return _allCategory.Count;
    }

    /// <summary>
    /// Searches for categories with pagination.
    /// </summary>
    /// <param name="categoryFilterDto">The filters to apply.</param>
    /// <param name="sortDto">The sorting options to apply.</param>
    /// <param name="page">The page number to retrieve.</param>
    /// <param name="perPage">The number of categories per page.</param>
    /// <returns>A pagination object containing the search results.</returns>
    public Pagination<Category> Search(CategoryFilterDto categoryFilterDto, SortDto sortDto, int page, int perPage)
    {
        var _allCategories = GetAll();
        var pagination = new Pagination<Category>();

        var filteredCategories = _allCategories.AsQueryable();

        if (!string.IsNullOrEmpty(categoryFilterDto.Name))
        {
            filteredCategories = filteredCategories.Where(x => x.Name != null && x.Name.Contains(categoryFilterDto.Name));
        }

        if (!string.IsNullOrEmpty(categoryFilterDto.Desc))
        {
            filteredCategories = filteredCategories.Where(x => x.Description != null && x.Description.Contains(categoryFilterDto.Desc));
        }

        //if (!string.IsNullOrEmpty(categoryFilterDto.Status))
        //{
        //    filteredCategories = filteredCategories.Where(x => x.Status != null && x.Status == categoryFilterDto.Status);
        //}

        Func<Category, object> keySelector = x => sortDto.SortBy switch
        {
            "Id" => x.Id,
            "Name" => x.Name ?? string.Empty,
            "Desc" => x.Description ?? string.Empty,
            //"Status" => x.Status ?? string.Empty,
            "CreatedBy" => x.CreatedBy ?? string.Empty,
            _ => x.CreatedAt ?? new DateTime()
        };
        pagination.Data = (sortDto.SortType == "DESC"
            ? filteredCategories.OrderByDescending(keySelector)
            : filteredCategories.OrderBy(keySelector))
            .Skip((page - 1) * perPage)
            .Take(perPage)
            .ToList();
        pagination.Page = page;
        pagination.PerPage = perPage;
        pagination.TotalRecord = filteredCategories.Count();
        pagination.TotalPage = (int)Math.Ceiling((double)filteredCategories.Count() / perPage);
        return pagination;
    }

    public Task<Category?> InsertAsync(long storeId, Category category) => throw new NotImplementedException();
    public Task<List<Category>> GetAllAsync(long storeId) => throw new NotImplementedException();
    public Task<Category?> GetByIdAsync(long storeId, long id) => throw new NotImplementedException();
    public Task<Pagination<Category>> GetWithPaginationAsync(long storeId, CategoryFilterDto categoryFilterDto, SortDto sortDto, int page, int perPage) => throw new NotImplementedException();
    public Task<Category?> UpdateByIdAsync(long storeId, Category category) => throw new NotImplementedException();
    public Task<Category?> DeleteByIdAsync(long storeId, long id, Staff author) => throw new NotImplementedException();
    public Task<List<Category>> DeleteByIdsAsync(long storeId, List<long> ids, Staff author) => throw new NotImplementedException();
    public Task<long> CountAsync(long storeId, CategoryFilterDto categoryFilterDto) => throw new NotImplementedException();
}
