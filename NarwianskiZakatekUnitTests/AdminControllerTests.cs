using Moq;
using NarwianskiZakatek.Controllers;
using NarwianskiZakatek.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using NarwianskiZakatek.ViewModels;

namespace NarwianskiZakatekUnitTests
{
    [TestClass]
    public class AdminControllerTests
    {
        public MockData _mockData;
        public AdminControllerTests()
        {
            _mockData = new MockData();
        }

        [TestMethod]
        public void AddToRole_Success()
        {
            var mockUsersService = new Mock<IUsersService>();
            mockUsersService.Setup(m => m.AddToRole(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(true);

            var controller = new AdminController(mockUsersService.Object);
            var response = controller.AddToRole("", "") as RedirectToActionResult;
            var expectedRedirectValues = new RouteValueDictionary
            {
                { "message", "Pomyślnie zmieniono rolę użytkownika." },
                { "action", "Users" }
            };
            Assert.AreEqual(expectedRedirectValues.Values.ElementAt(1), response.ActionName);
            Assert.AreEqual(expectedRedirectValues.Values.ElementAt(0), response.RouteValues.ElementAt(0).Value);
        }

        [TestMethod]
        public void AddToRole_Failure()
        {
            var mockUsersService = new Mock<IUsersService>();
            mockUsersService.Setup(m => m.AddToRole(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(false);

            var controller = new AdminController(mockUsersService.Object);
            var response = controller.AddToRole("", "") as RedirectToActionResult;
            var expectedRedirectValues = new RouteValueDictionary
            {
                { "message", "Wystąpił błąd podczas nadawania uprawnień użytkownikowi." },
                { "action", "Users" }
            };
            Assert.AreEqual(expectedRedirectValues.Values.ElementAt(1), response.ActionName);
            Assert.AreEqual(expectedRedirectValues.Values.ElementAt(0), response.RouteValues.ElementAt(0).Value);
        }

        [TestMethod]
        public void RemoveFromRole_Success()
        {
            var mockUsersService = new Mock<IUsersService>();
            mockUsersService.Setup(m => m.RemoveFromRole(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(true);

            var controller = new AdminController(mockUsersService.Object);
            var response = controller.RemoveFromRole("", "") as RedirectToActionResult;
            var expectedRedirectValues = new RouteValueDictionary
            {
                { "message", "Pomyślnie zmieniono rolę użytkownika." },
                { "action", "Users" }
            };
            Assert.AreEqual(expectedRedirectValues.Values.ElementAt(1), response.ActionName);
            Assert.AreEqual(expectedRedirectValues.Values.ElementAt(0), response.RouteValues.ElementAt(0).Value);
        }

        [TestMethod]
        public void RemoveFromRole_Failure()
        {
            var mockUsersService = new Mock<IUsersService>();
            mockUsersService.Setup(m => m.RemoveFromRole(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(false);

            var controller = new AdminController(mockUsersService.Object);
            var response = controller.RemoveFromRole("", "") as RedirectToActionResult;
            var expectedRedirectValues = new RouteValueDictionary
            {
                { "message", "Wystąpił błąd podczas wycofywania uprawnień użytkownika." },
                { "action", "Users" }
            };
            Assert.AreEqual(expectedRedirectValues.Values.ElementAt(1), response.ActionName);
            Assert.AreEqual(expectedRedirectValues.Values.ElementAt(0), response.RouteValues.ElementAt(0).Value);
        }

        [TestMethod]
        public void SendWarningGet()
        {
            var mockUsersService = new Mock<IUsersService>();

            var controller = new AdminController(mockUsersService.Object);
            var response = controller.SendWarning("user") as ViewResult;
            Assert.AreEqual("user", ((WarningViewModel)response.Model).UserName);
        }

        [TestMethod]
        public void SendWarningPost()
        {
            WarningViewModel warning = new WarningViewModel()
            {
                UserName = "user",
                Message = "message"
            };
            var mockUsersService = new Mock<IUsersService>();

            var controller = new AdminController(mockUsersService.Object);
            var response = controller.SendWarning(warning) as RedirectToActionResult;
            var expectedRedirectValues = new RouteValueDictionary
            {
                { "message", "Wysłano ostrzeżenie użytkownikowi." },
                { "action", "Users" }
            };
            Assert.AreEqual(expectedRedirectValues.Values.ElementAt(1), response.ActionName);
            Assert.AreEqual(expectedRedirectValues.Values.ElementAt(0), response.RouteValues.ElementAt(0).Value);
        }

        [TestMethod]
        public void LockAccountPost()
        {
            var mockUsersService = new Mock<IUsersService>();

            var controller = new AdminController(mockUsersService.Object);
            var response = controller.LockAccount("") as RedirectToActionResult;
            var expectedRedirectValues = new RouteValueDictionary
            {
                { "message", "Konto użytkownika zostało zablokowane." },
                { "action", "Users" }
            };
            Assert.AreEqual(expectedRedirectValues.Values.ElementAt(1), response.ActionName);
            Assert.AreEqual(expectedRedirectValues.Values.ElementAt(0), response.RouteValues.ElementAt(0).Value);
        }

        [TestMethod]
        public void UnlockAccountPost()
        {
            var mockUsersService = new Mock<IUsersService>();

            var controller = new AdminController(mockUsersService.Object);
            var response = controller.UnlockAccount("") as RedirectToActionResult;
            var expectedRedirectValues = new RouteValueDictionary
            {
                { "message", "Konto użytkownika zostało odblokowane." },
                { "action", "Users" }
            };
            Assert.AreEqual(expectedRedirectValues.Values.ElementAt(1), response.ActionName);
            Assert.AreEqual(expectedRedirectValues.Values.ElementAt(0), response.RouteValues.ElementAt(0).Value);
        }
    }
}
