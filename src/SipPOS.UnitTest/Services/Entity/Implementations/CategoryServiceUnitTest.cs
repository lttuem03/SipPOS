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
    public class CategoryServiceUnitTest
    {

        public CategoryService categoryService = App.GetService<CategoryService>();


        [TestMethod]
        public async void GetCategoryById_WhenCategoryExists_ShouldReturnCategory()
        {
            var categoryId = 1;

            var category = await categoryService.GetById(categoryId);

            Assert.IsNotNull(category);
            Assert.AreEqual(categoryId, category.Id);
        }

        [TestMethod]
        public async void GetCategoryById_WhenCategoryDoesNotExist_ShouldReturnNull()
        {
            var categoryId = -1;

            var category = await categoryService.GetById(categoryId);

            Assert.IsNull(category);
        }

        [TestMethod]
        public async void GetCategorys_WhenCalled_ShouldReturnCategoryList()
        {
            var categories = await categoryService.GetAll();

            Assert.IsNotNull(categories);
            Assert.IsTrue(categories.Count != 0);

        }

        [TestMethod]
        public async void DeleteCategory_WhenCategoryExists_ShouldDeleteCategory()
        {
            var categoryDto = new CategoryDto
            {
                Name = "Test Category",
                Description = "Test Description"
            };

            categoryDto = await categoryService.Insert(categoryDto);

            categoryDto = await categoryService.DeleteById((long)categoryDto.Id);

            Assert.IsNotNull(categoryDto.DeletedAt);
        }

        [TestMethod]
        public async void DeleteCategory_WhenCategoryDoesNotExist_ShouldNotDeleteCategory()
        {
            var categoryId = -1;

            var category = await categoryService.DeleteById(categoryId);

            Assert.IsNull(category);
        }

        [TestMethod]
        public async void AddCategory_WhenCategoryIsValid_ShouldAddCategory()
        {
            var category = new CategoryDto
            {
                Name = "Test Category",
                Description = "Test Description"
            };

            var categoryDto = await categoryService.Insert(category);

            Assert.IsNotNull(categoryDto);
            Assert.AreEqual(category.Name, categoryDto.Name);
            Assert.AreEqual(category.Description, categoryDto.Description);

            await categoryService.DeleteById((long)category.Id);
        }

    }
}
