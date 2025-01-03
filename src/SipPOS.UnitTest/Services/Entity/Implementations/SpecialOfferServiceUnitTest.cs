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
    public class SpecialOfferServiceUnitTest
    {

        public SpecialOfferService specialOfferService = App.GetService<SpecialOfferService>();


        [TestMethod]
        public async void GetSpecialOfferById_WhenSpecialOfferExists_ShouldReturnSpecialOffer()
        {
            var specialOfferId = 1;

            var specialOffer = await specialOfferService.GetById(specialOfferId);

            Assert.IsNotNull(specialOffer);
            Assert.AreEqual(specialOfferId, specialOffer.Id);
        }

        [TestMethod]
        public async void GetSpecialOfferById_WhenSpecialOfferDoesNotExist_ShouldReturnNull()
        {
            var specialOfferId = -1;

            var specialOffer = await specialOfferService.GetById(specialOfferId);

            Assert.IsNull(specialOffer);
        }

        [TestMethod]
        public async void GetSpecialOffers_WhenCalled_ShouldReturnSpecialOfferList()
        {
            var categorys = await specialOfferService.GetAll();

            Assert.IsNotNull(categorys);
            Assert.IsTrue(categorys.Count != 0);

        }

        [TestMethod]
        public async void DeleteSpecialOffer_WhenSpecialOfferExists_ShouldDeleteSpecialOffer()
        {
            var specialOfferDto = new SpecialOfferDto
            {
                Name = "Test SpecialOffer",
                Description = "Test Description"
            };

            specialOfferDto = await specialOfferService.Insert(specialOfferDto);

            specialOfferDto = await specialOfferService.DeleteById((long)specialOfferDto.Id);

            Assert.IsNotNull(specialOfferDto.DeletedAt);
        }

        [TestMethod]
        public void DeleteSpecialOffer_WhenSpecialOfferDoesNotExist_ShouldNotDeleteSpecialOffer()
        {
            var specialOfferId = -1;

            var specialOffer = specialOfferService.DeleteById(specialOfferId);

            Assert.IsNull(specialOffer);
        }

        [TestMethod]
        public async void AddSpecialOffer_WhenSpecialOfferIsValid_ShouldAddSpecialOffer()
        {
            var specialOffer = new SpecialOfferDto
            {
                Name = "Test SpecialOffer",
                Description = "Test Description"
            };

            var specialOfferDto = await specialOfferService.Insert(specialOffer);

            Assert.IsNotNull(specialOfferDto);
            Assert.AreEqual(specialOffer.Name, specialOfferDto.Name);
            Assert.AreEqual(specialOffer.Description, specialOfferDto.Description);

            await specialOfferService.DeleteById((long)specialOffer.Id);
        }

    }
}
