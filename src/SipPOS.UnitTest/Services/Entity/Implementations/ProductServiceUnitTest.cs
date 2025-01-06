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
        public async void GetProductById_WhenProductExists_ShouldReturnProduct()
        {
            var productId = 1;

            var product = await productService.GetById(productId);

            Assert.IsNotNull(product);
            Assert.AreEqual(productId, product.Id);
        }

        [TestMethod]
        public async void GetProductById_WhenProductDoesNotExist_ShouldReturnNull()
        {
            var productId = -1;

            var product = await productService.GetById(productId);

            Assert.IsNull(product);
        }

        [TestMethod]
        public async void GetProducts_WhenCalled_ShouldReturnProductList()
        {
            var products = await productService.GetAll();

            Assert.IsNotNull(products);
            Assert.IsTrue(products.Count != 0);
        }

        [TestMethod]
        public async void DeleteProduct_WhenProductExists_ShouldDeleteProduct()
        {
            var productDto = new ProductDto
            {
                Name = "Test Product",
                Description = "Test Description",
                CategoryId = 1
            };

            productDto = await productService.Insert(productDto);

            productDto = await productService.DeleteById((long)productDto.Id);

            Assert.IsNotNull(productDto.DeletedAt);
        }

        [TestMethod]
        public async void DeleteProduct_WhenProductDoesNotExist_ShouldNotDeleteProduct()
        {
            var productId = -1;

            var product = await productService.DeleteById(productId);

            Assert.IsNull(product);
        }

        [TestMethod]
        public async void AddProduct_WhenProductIsValid_ShouldAddProduct()
        {
            var product = new ProductDto
            {
                Name = "Test Product",
                Description = "Test Description",
                CategoryId = 1
            };

            var productDto = await productService.Insert(product);

            Assert.IsNotNull(productDto);
            Assert.AreEqual(product.Name, productDto.Name);
            Assert.AreEqual(product.Description, productDto.Description);

            await productService.DeleteById((long)product.Id);
        }

    }
}
