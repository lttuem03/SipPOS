using SipPOS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SipPOS.DataAccess.Interfaces;

namespace SipPOS.DataAccess.Implementations
{
    class MockProductDao : IProductDao
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

        public Product DeleteById(long Id)
        {
            Product product = GetById(Id);
            if (product != null)
            {
                product.DeletedAt = DateTime.Now;
            }
            return product;
        }

        public IEnumerable<Product> GetAll()
        {
            return _allProducts.Where(_allProducts => _allProducts.DeletedAt == null).ToList();
        }

        public Product GetById(long Id)
        {
            return _allProducts.Find(x => x.Id == Id && x.DeletedAt != null);
        }

        public Product Insert(Product product)
        {
            _allProducts.Add(product);
            return product;
        }

        public Product UpdateById(Product product)
        {
            Product oldProduct = GetById(product.Id);
            if (oldProduct != null)
            {
                oldProduct.Name = product.Name;
                oldProduct.Desc = product.Desc;
                oldProduct.Price = product.Price;
                oldProduct.Status = product.Status;
                oldProduct.SymbolCode = product.SymbolCode;
                oldProduct.SymbolName = product.SymbolName;
            }
            return oldProduct;
        }
    }
}
