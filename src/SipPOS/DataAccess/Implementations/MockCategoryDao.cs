using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SipPOS.Models;
using SipPOS.DataAccess.Interfaces;
using SipPOS.DataTransfer;

namespace SipPOS.DataAccess.Implementations;

class MockCategoryDao : ICategoryDao
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

    public Category? DeleteById(long id)
    {
        Category? category = GetById(id);

        if (category != null)
        {
            _allCategory.Remove(category);
        }

        return category;
    }

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

    public IList<Category> GetAll()
    {
        return _allCategory.Where(x => x.DeletedAt == null).ToList();
    }

    public Category? GetById(long id)
    {
        return _allCategory.FirstOrDefault(x => x.Id == id);
    }

    public Category? Insert(Category category)
    {
        _allCategory.Add(category);
        category.CreatedAt = DateTime.Now;
        category.CreatedBy = "admin";
        return category;
    }

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

    public long Count()
    {
        return _allCategory.Count;
    }

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
            "Id" => x.Id ?? 0,
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
