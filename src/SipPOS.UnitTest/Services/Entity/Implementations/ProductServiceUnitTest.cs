using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Controls;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting.AppContainer;
using SipPOS.DataTransfer.Entity;
using SipPOS.Services.Entity.Implementations;

namespace SipPOS.UnitTest.Services.Entity.Implementations
{

    [TestClass]
    public class ProductServiceUnitTest
    {
        public ProductService productService = App.GetService<ProductService>();


        [TestMethod]
        public void GetProductById_WhenProductExists_ShouldReturnProduct()
        {
            var productId = 1;

            var product = productService.GetById(productId);

            Assert.IsNotNull(product);
            Assert.AreEqual(productId, product.Id);
        }

        [TestMethod]
        public void GetProductById_WhenProductDoesNotExist_ShouldReturnNull()
        {
            var productId = -1;

            var product = productService.GetById(productId);

            Assert.IsNull(product);
        }

        [TestMethod]
        public void GetProducts_WhenCalled_ShouldReturnProductList()
        {

            var products = productService.GetAll();

            Assert.IsNotNull(products);
            Assert.IsTrue(products.Count != 0);

        }

        [TestMethod]
        public void DeleteProduct_WhenProductExists_ShouldDeleteProduct()
        {
            var productId = 1;

            var product = productService.DeleteById(productId);

            Assert.IsNotNull(product.DeletedAt);
        }

        [TestMethod]
        public void DeleteProduct_WhenProductDoesNotExist_ShouldNotDeleteProduct()
        {
            var productId = -1;

            var product = productService.DeleteById(productId);

            Assert.IsNull(product);
        }

        [TestMethod]
        public void AddProduct_WhenProductIsValid_ShouldAddProduct()
        {
            var product = new ProductDto
            {
                Name = "Test Product",
                Desc = "Test Description",
                Price = 10.0,
                CategoryId = 1
            };

            var productDto = productService.Insert(product);

            Assert.IsNotNull(productDto);
            Assert.AreEqual(product.Name, productDto.Name);
            Assert.AreEqual(product.Desc, productDto.Desc);

            productService.DeleteById((long)product.Id);
        }

    }
}
