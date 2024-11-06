using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SipPOS.Models;
using SipPOS.DataAccess.Interfaces;
using SipPOS.DataTransfer;

namespace SipPOS.DataAccess.Implementations;

/// <summary>
/// Mock implementation of the ICategoryDao interface for testing purposes.
/// Providing mock data for a few Category objects.
/// </summary>
public class MockCategoryDao : ICategoryDao
{
    private readonly List<Category> _allCategory =
    [
        new()
        {
            Id = 1,
            Name = "Nước giải khát",
            Desc = "Đây là danh mục nước giải khát",
            Status = "Available",
            ImageUrls = new List<string> {
                "https://www.austriajuice.com/hs-fs/hubfs/Beverage_compounds_drinks.jpg?width=730&name=Beverage_compounds_drinks.jpg",
                "https://s7d1.scene7.com/is/image/KeminIndustries/shutterstock_519547867?$responsive$"
            },
            CreatedAt = new DateTime(2022, 5, 15),
            CreatedBy = "admin",
        },
        new()
        {
            Id = 2,
            Name = "Nước trái cây",
            Desc = "Danh mục các loại nước ép trái cây",
            Status = "Available",
            ImageUrls = new List<string> {
                "https://www.austriajuice.com/hs-fs/hubfs/Beverage_compounds_drinks.jpg?width=730&name=Beverage_compounds_drinks.jpg",
                "https://s7d1.scene7.com/is/image/KeminIndustries/shutterstock_519547867?$responsive$"
            },
            CreatedAt = new DateTime(2022, 5, 15),
            CreatedBy = "admin",
        },
        new()
        {
            Id = 3,
            Name = "Nước suối",
            Desc = "Danh mục các loại nước suối",
            Status = "Available",
            ImageUrls = new List<string> {
                "https://www.austriajuice.com/hs-fs/hubfs/Beverage_compounds_drinks.jpg?width=730&name=Beverage_compounds_drinks.jpg",
                "https://s7d1.scene7.com/is/image/KeminIndustries/shutterstock_519547867?$responsive$"
            },
            CreatedAt = new DateTime(2022, 5, 15),
            CreatedBy = "admin",
        },
        new()
        {
            Id = 4,
            Name = "Nước ngọt",
            Desc = "Danh mục các loại nước ngọt",
            Status = "Available",
            ImageUrls = new List<string> {
                "https://www.austriajuice.com/hs-fs/hubfs/Beverage_compounds_drinks.jpg?width=730&name=Beverage_compounds_drinks.jpg",
                "https://s7d1.scene7.com/is/image/KeminIndustries/shutterstock_519547867?$responsive$"
            },
            CreatedAt = new DateTime(2022, 5, 15),
            CreatedBy = "admin",
        },
        new()
        {
            Id = 5,
            Name = "Nước tăng lực",
            Desc = "Danh mục các loại nước tăng lực",
            Status = "Available",
            ImageUrls = new List<string> {
                "https://www.austriajuice.com/hs-fs/hubfs/Beverage_compounds_drinks.jpg?width=730&name=Beverage_compounds_drinks.jpg",
                "https://s7d1.scene7.com/is/image/KeminIndustries/shutterstock_519547867?$responsive$"
            },
            CreatedAt = new DateTime(2022, 5, 15),
            CreatedBy = "admin",
        },
        new()
        {
            Id = 6,
            Name = "Nước ép",
            Desc = "Danh mục các loại nước ép",
            Status = "Available",
            ImageUrls = new List<string> {
                "https://www.austriajuice.com/hs-fs/hubfs/Beverage_compounds_drinks.jpg?width=730&name=Beverage_compounds_drinks.jpg",
                "https://s7d1.scene7.com/is/image/KeminIndustries/shutterstock_519547867?$responsive$"
            },
            CreatedAt = new DateTime(2022, 5, 15),
            CreatedBy = "admin",
        },
        new()
        {
            Id = 7,
            Name = "Nước lọc",
            Desc = "Danh mục các loại nước lọc",
            Status = "Available",
            ImageUrls = new List<string> {
                "https://www.austriajuice.com/hs-fs/hubfs/Beverage_compounds_drinks.jpg?width=730&name=Beverage_compounds_drinks.jpg",
                "https://s7d1.scene7.com/is/image/KeminIndustries/shutterstock_519547867?$responsive$"
            },
            CreatedAt = new DateTime(2022, 5, 15),
            CreatedBy = "admin",
        },
        new()
        {
            Id = 8,
            Name = "Nước đóng chai",
            Desc = "Danh mục các loại nước đóng chai",
            Status = "Available",
            ImageUrls = new List<string> {
                "https://www.austriajuice.com/hs-fs/hubfs/Beverage_compounds_drinks.jpg?width=730&name=Beverage_compounds_drinks.jpg",
                "https://s7d1.scene7.com/is/image/KeminIndustries/shutterstock_519547867?$responsive$"
            },
            CreatedAt = new DateTime(2022, 5, 15),
            CreatedBy = "admin",
        },
        new()
        {
            Id = 9,
            Name = "Nước đóng lon",
            Desc = "Danh mục các loại nước đóng lon",
            Status = "Available",
            CreatedAt = new DateTime(2022, 5, 15),
            CreatedBy = "admin",
        },
        new()
        {
            Id = 10,
            Name = "Nước đóng gói",
            Desc = "Danh mục các loại nước đóng gói",
            Status = "Available",
            CreatedAt = new DateTime(2022, 5, 15),
            CreatedBy = "admin",
        },
        new()
        {
            Id = 11,
            Name = "Nước đóng hộp",
            Desc = "Danh mục các loại nước đóng hộp",
            Status = "Available",
            ImageUrls = new List<string> {
                "https://www.austriajuice.com/hs-fs/hubfs/Beverage_compounds_drinks.jpg?width=730&name=Beverage_compounds_drinks.jpg",
                "https://s7d1.scene7.com/is/image/KeminIndustries/shutterstock_519547867?$responsive$"
            },
            CreatedAt = new DateTime(2022, 5, 15),
            CreatedBy = "admin",
        },
    ];

    /// <summary>
    /// Deletes a category by its ID.
    /// </summary>
    /// <param name="id">The ID of the category to delete.</param>
    /// <returns>The deleted category if found; otherwise, null.</returns>
    public Category? DeleteById(long id)
    {
        Category? category = GetById(id);

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
        List<Category> products = new List<Category>();

        foreach (var id in ids)
        {
            Category? deletedCategory = DeleteById(id);

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
            oldCategory.Desc = category.Desc;
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
        IList<Category> _allCategories = GetAll();
        Pagination<Category> pagination = new Pagination<Category>();

        var filteredCategories = _allCategories.AsQueryable();

        if (!string.IsNullOrEmpty(categoryFilterDto.Name))
        {
            filteredCategories = filteredCategories.Where(x => x.Name != null && x.Name.Contains(categoryFilterDto.Name));
        }

        if (!string.IsNullOrEmpty(categoryFilterDto.Desc))
        {
            filteredCategories = filteredCategories.Where(x => x.Desc != null && x.Desc.Contains(categoryFilterDto.Desc));
        }

        if (!string.IsNullOrEmpty(categoryFilterDto.Status))
        {
            filteredCategories = filteredCategories.Where(x => x.Status != null && x.Status == categoryFilterDto.Status);
        }

        Func<Category, object> keySelector = x => sortDto.SortBy switch
        {
            "Id" => x.Id,
            "Name" => x.Name ?? string.Empty,
            "Desc" => x.Desc ?? string.Empty,
            "Status" => x.Status ?? string.Empty,
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
}
