using SipPOS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SipPOS.DataAccess.Interfaces;

namespace SipPOS.DataAccess.Implementations
{
    class MockCategoryDao : ICategoryDao
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

        public Category DeleteById(long Id)
        {
            Category category = GetById(Id);
            if (category != null)
            {
                _allCategory.Remove(category);
            }
            return category;
        }

        public IEnumerable<Category> GetAll()
        {
            return _allCategory.Where(x => x.DeletedAt == null).ToList();
        }

        public Category GetById(long Id)
        {
            return _allCategory.Find(x => x.Id == Id && x.DeletedAt != null);
        }

        public Category Insert(Category category)
        {
            _allCategory.Add(category);
            return category;
        }

        public Category UpdateById(Category category)
        {
            var oldCategory = _allCategory.FirstOrDefault(x => x.Id == category.Id);
            if (oldCategory != null)
            {
                oldCategory.Name = category.Name;
                oldCategory.Desc = category.Desc;
            }
            return oldCategory;
        }
    }
}
