using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NarwianskiZakatek.Controllers;
using NarwianskiZakatek.Models;
using NarwianskiZakatek.Repositories;
using NarwianskiZakatek.ViewModels;

namespace NarwianskiZakatekUnitTests.controllers
{
    [TestClass]
    public class HomeControllerTests
    {
        [TestMethod]
        public void GetAccomodation_ReturnsView()
        {
            var mockLogger = new Mock<ILogger<HomeController>>().Object;
            var mockDescriptionService = new Mock<IDescriptionsService>();
            mockDescriptionService.Setup(m => m.GetByTitle(It.IsAny<string>())).Returns(new Description()
            {
                Title = "Noclegi",
                Content = "XXXXXXX"
            });

            var controller = new HomeController(mockLogger, mockDescriptionService.Object);
            var result = controller.Accomodation() as ViewResult;
            var description = (Description)result.ViewData.Model;
            Assert.AreEqual("Noclegi", description.Title);
            Assert.AreEqual("XXXXXXX", description.Content);
        }

        [TestMethod]
        public void GetAbout_ReturnsView()
        {
            var mockLogger = new Mock<ILogger<HomeController>>().Object;
            var mockDescriptionService = new Mock<IDescriptionsService>();
            mockDescriptionService.Setup(m => m.GetByTitle(It.IsAny<string>())).Returns(new Description()
            {
                Title = "O nas",
                Content = "XXXXXXX"
            });

            var controller = new HomeController(mockLogger, mockDescriptionService.Object);
            var result = controller.About() as ViewResult;
            var description = (Description)result.ViewData.Model;
            Assert.AreEqual("O nas", description.Title);
            Assert.AreEqual("XXXXXXX", description.Content);
        }

        [TestMethod]
        public void GetAttractions_ReturnsView()
        {
            var mockLogger = new Mock<ILogger<HomeController>>().Object;
            var mockDescriptionService = new Mock<IDescriptionsService>();
            mockDescriptionService.Setup(m => m.GetByTitle(It.IsAny<string>())).Returns(new Description()
            {
                Title = "Atrakcje",
                Content = "XXXXXXX"
            });

            var controller = new HomeController(mockLogger, mockDescriptionService.Object);
            var result = controller.Attractions() as ViewResult;
            var description = (Description)result.ViewData.Model;
            Assert.AreEqual("Atrakcje", description.Title);
            Assert.AreEqual("XXXXXXX", description.Content);
        }

        [TestMethod]
        public void GetCatering_ReturnsView()
        {
            var mockLogger = new Mock<ILogger<HomeController>>().Object;
            var mockDescriptionService = new Mock<IDescriptionsService>();
            mockDescriptionService.Setup(m => m.GetByTitle(It.IsAny<string>())).Returns(new Description()
            {
                Title = "Restauracja",
                Content = "XXXXXXX"
            });

            var controller = new HomeController(mockLogger, mockDescriptionService.Object);
            var result = controller.Catering() as ViewResult;
            var description = (Description)result.ViewData.Model;
            Assert.AreEqual("Restauracja", description.Title);
            Assert.AreEqual("XXXXXXX", description.Content);
        }

        [TestMethod]
        public void GetNeighborhood_ReturnsView()
        {
            var mockLogger = new Mock<ILogger<HomeController>>().Object;
            var mockDescriptionService = new Mock<IDescriptionsService>();
            mockDescriptionService.Setup(m => m.GetByTitle(It.IsAny<string>())).Returns(new Description()
            {
                Title = "Okolica",
                Content = "XXXXXXX"
            });

            var controller = new HomeController(mockLogger, mockDescriptionService.Object);
            var result = controller.Neighborhood() as ViewResult;
            var description = (Description)result.ViewData.Model;
            Assert.AreEqual("Okolica", description.Title);
            Assert.AreEqual("XXXXXXX", description.Content);
        }

        [TestMethod]
        public void GetEdit_ReturnsView()
        {
            var mockLogger = new Mock<ILogger<HomeController>>().Object;
            var mockDescriptionService = new Mock<IDescriptionsService>();
            mockDescriptionService.Setup(m => m.GetByTitle(It.IsAny<string>())).Returns(new Description()
            {
                Title = "Okolica",
                Content = "XXXXXXX"
            });

            var controller = new HomeController(mockLogger, mockDescriptionService.Object);
            var result = controller.Edit("Okolica") as ViewResult;
            var description = (DescriptionViewModel)result.ViewData.Model;
            Assert.AreEqual("Okolica", description.Title);
            Assert.AreEqual("XXXXXXX", description.Content);
        }

        [TestMethod]
        public void PostEdit_ValidationPassed()
        {
            var mockLogger = new Mock<ILogger<HomeController>>().Object;
            var mockDescriptionService = new Mock<IDescriptionsService>();
            mockDescriptionService.Setup(m => m.GetByTitle(It.IsAny<string>())).Returns(new Description()
            {
                Title = "Okolica",
                Content = "XXXXXXX"
            });
            var viewModel = new DescriptionViewModel()
            {
                Title = "Okolica",
                Content = "XXXXXXX"
            };


            var controller = new HomeController(mockLogger, mockDescriptionService.Object);
            var result = controller.Edit(viewModel) as ViewResult;
            var description = (DescriptionViewModel)result.ViewData.Model;
            Assert.AreEqual("Okolica", description.Title);
            Assert.AreEqual("XXXXXXX", description.Content);
        }

        //post edit
        [TestMethod]
        public void PostEdit_ValidationFails()
        {
            var mockLogger = new Mock<ILogger<HomeController>>().Object;
            var mockDescriptionService = new Mock<IDescriptionsService>();
            mockDescriptionService.Setup(m => m.GetByTitle(It.IsAny<string>())).Returns(new Description()
            {
                Title = "Okolica",
                Content = "XXXXXXX"
            });
            var viewModel = new DescriptionViewModel()
            {
                Title = "Okolica",
                Content = ""
            };
            var controller = new HomeController(mockLogger, mockDescriptionService.Object);
            controller.ModelState.AddModelError("test", "test");
            var result = controller.Edit(viewModel) as ViewResult;
            var description = (DescriptionViewModel)result.ViewData.Model;
            Assert.AreEqual("Okolica", description.Title);
            Assert.AreEqual("XXXXXXX", description.Content);
        }
    }
}
