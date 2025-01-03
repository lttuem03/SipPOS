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
        public void GetCategoryById_WhenCategoryExists_ShouldReturnCategory()
        {
            var categoryId = 1;

            var category = categoryService.GetById(categoryId);

            Assert.IsNotNull(category);
            Assert.AreEqual(categoryId, category.Id);
        }

        [TestMethod]
        public void GetCategoryById_WhenCategoryDoesNotExist_ShouldReturnNull()
        {
            var categoryId = -1;

            var category = categoryService.GetById(categoryId);

            Assert.IsNull(category);
        }

        [TestMethod]
        public void GetCategorys_WhenCalled_ShouldReturnCategoryList()
        {
            var categorys = categoryService.GetAll();

            Assert.IsNotNull(categorys);
            Assert.IsTrue(categorys.Count != 0);

        }

        [TestMethod]
        public void DeleteCategory_WhenCategoryExists_ShouldDeleteCategory()
        {
            var categoryDto = new CategoryDto
            {
                Name = "Test Category",
                Desc = "Test Description"
            };

            categoryDto = categoryService.Insert(categoryDto);

            categoryDto = categoryService.DeleteById((long)categoryDto.Id);

            Assert.IsNotNull(categoryDto.DeletedAt);
        }

        [TestMethod]
        public void DeleteCategory_WhenCategoryDoesNotExist_ShouldNotDeleteCategory()
        {
            var categoryId = -1;

            var category = categoryService.DeleteById(categoryId);

            Assert.IsNull(category);
        }

        [TestMethod]
        public void AddCategory_WhenCategoryIsValid_ShouldAddCategory()
        {
            var category = new CategoryDto
            {
                Name = "Test Category",
                Desc = "Test Description"
            };

            var categoryDto = categoryService.Insert(category);

            Assert.IsNotNull(categoryDto);
            Assert.AreEqual(category.Name, categoryDto.Name);
            Assert.AreEqual(category.Desc, categoryDto.Desc);

            categoryService.DeleteById((long)category.Id);
        }

    }
}
