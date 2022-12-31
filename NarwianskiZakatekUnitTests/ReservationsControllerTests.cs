using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Moq;
using NarwianskiZakatek.Controllers;
using NarwianskiZakatek.Models;
using NarwianskiZakatek.Repositories;
using NarwianskiZakatek.ViewModels;
using System.Web.Mvc;
using ViewResult = Microsoft.AspNetCore.Mvc.ViewResult;

namespace NarwianskiZakatekUnitTests
{
    [TestClass]
    public class ReservationsControllerTests
    {
        public MockData _mockData;
        public ReservationsControllerTests()
        {
            _mockData = new MockData();
        }

        [TestMethod]
        public void GetDetailsFull()
        {
            var mockUserService = new Mock<IUsersService>();
            var mockService = new Mock<IReservationsService>();
            mockService.Setup(m => m.GetReservation(It.IsAny<string>()))
                .Returns(_mockData.reservations.First());
            mockService.Setup(m => m.GetRoomsByReservation(It.IsAny<Reservation>()))
                .Returns(_mockData.rooms);

            var controller = new ReservationsController(mockService.Object, mockUserService.Object);
            var result = controller.DetailsFull("id") as ViewResult;

            Assert.AreEqual(_mockData.reservations.First(), (Reservation)result.Model);
            Assert.AreEqual(_mockData.rooms, controller.ViewBag.Rooms);
        }

        [TestMethod]
        public void GetDate()
        {
            var mockUserService = new Mock<IUsersService>();
            var mockService = new Mock<IReservationsService>();

            var controller = new ReservationsController(mockService.Object, mockUserService.Object);
            controller.Date("msg");

            Assert.AreEqual("msg", controller.ViewBag.Message);
        }

        [TestMethod]
        public void PostNew_PastBeginDate()
        {
            var mockUserService = new Mock<IUsersService>();
            var mockService = new Mock<IReservationsService>();

            var controller = new ReservationsController(mockService.Object, mockUserService.Object);
            var result = controller.New(DateTime.Today.AddDays(-1), DateTime.Today.AddDays(1)) as RedirectToActionResult;
            var expectedRedirectValues = new RouteValueDictionary
            {
                { "message", "Nie możesz rezerwować pobytu z datą przeszłą." },
                { "action", "Date" }
            };
            Assert.AreEqual(expectedRedirectValues.Values.ElementAt(1), result.ActionName);
            Assert.AreEqual(expectedRedirectValues.Values.ElementAt(0), result.RouteValues.ElementAt(0).Value);
        }

        [TestMethod]
        public void PostNew_EndDateEarlierThanBegin()
        {
            var mockUserService = new Mock<IUsersService>();
            var mockService = new Mock<IReservationsService>();

            var controller = new ReservationsController(mockService.Object, mockUserService.Object);
            var result = controller.New(DateTime.Today.AddDays(1), DateTime.Today.AddDays(-1)) as RedirectToActionResult;
            var expectedRedirectValues = new RouteValueDictionary
            {
                { "message", "Data zakończenia rezerwacji nie może być wcześniejsza, niż data rozpoczęcia." },
                { "action", "Date" }
            };
            Assert.AreEqual(expectedRedirectValues.Values.ElementAt(1), result.ActionName);
            Assert.AreEqual(expectedRedirectValues.Values.ElementAt(0), result.RouteValues.ElementAt(0).Value);
        }

        [TestMethod]
        public void PostNew_ValidData()
        {
            List<SelectListItem> rooms = _mockData.rooms.Select(r => new SelectListItem { Value = r.RoomId.ToString(), Text = r.ToString() }).ToList();
            var mockUserService = new Mock<IUsersService>();
            var mockService = new Mock<IReservationsService>();
            mockService.Setup(m => m.FindAvailableRooms(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(rooms);

            var controller = new ReservationsController(mockService.Object, mockUserService.Object);
            var result = controller.New(DateTime.Today.AddDays(1), DateTime.Today.AddDays(3)) as ViewResult;

            Assert.AreEqual(rooms, controller.ViewBag.RoomList);
            Assert.AreEqual(DateTime.Today.AddDays(1), ((RoomsViewModel)result.Model).BeginDate);
            Assert.AreEqual(DateTime.Today.AddDays(3), ((RoomsViewModel)result.Model).EndDate);
            Assert.AreEqual(DateTime.Today.AddDays(3), ((RoomsViewModel)result.Model).EndDate);
        }

        [TestMethod]
        public void GetDelete_NullId()
        {
            var mockService = new Mock<IReservationsService>();
            var mockUserService = new Mock<IUsersService>();

            var controller = new ReservationsController(mockService.Object, mockUserService.Object);
            var result = controller.Delete(null) as NotFoundResult;

            Assert.IsNotNull(result);
        }


        [TestMethod]
        public void GetDelete_WrongId()
        {
            var mockService = new Mock<IReservationsService>();
            mockService.Setup(m => m.GetReservation(It.IsAny<string>())).Returns((Reservation)null);
            var mockUserService = new Mock<IUsersService>();

            var controller = new ReservationsController(mockService.Object, mockUserService.Object);
            var result = controller.Delete("1") as NotFoundResult;

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void PostDelete_WrongId()
        {
            var mockService = new Mock<IReservationsService>();
            mockService.Setup(m => m.GetReservation(It.IsAny<string>())).Returns((Reservation)null);
            var mockUserService = new Mock<IUsersService>();

            var controller = new ReservationsController(mockService.Object, mockUserService.Object);
            var result = controller.DeletePost("1") as NotFoundResult;

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetDeleteAny_NullId()
        {
            var mockService = new Mock<IReservationsService>();
            var mockUserService = new Mock<IUsersService>();

            var controller = new ReservationsController(mockService.Object, mockUserService.Object);
            var result = controller.DeleteAny(null) as NotFoundResult;

            Assert.IsNotNull(result);
        }


        [TestMethod]
        public void GetDeleteAny_WrongId()
        {
            var mockService = new Mock<IReservationsService>();
            mockService.Setup(m => m.GetReservation(It.IsAny<string>())).Returns((Reservation)null);
            var mockUserService = new Mock<IUsersService>();

            var controller = new ReservationsController(mockService.Object, mockUserService.Object);
            var result = controller.DeleteAny("1") as NotFoundResult;

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetDeleteAny_ValidData()
        {
            var mockService = new Mock<IReservationsService>();
            mockService.Setup(m => m.GetReservation(It.IsAny<string>())).Returns(_mockData.reservations.First());
            var mockUserService = new Mock<IUsersService>();

            var controller = new ReservationsController(mockService.Object, mockUserService.Object);
            var result = controller.DeleteAny("1") as ViewResult;

            Assert.AreEqual(_mockData.reservations.First(), (Reservation)result.Model);
        }

        [TestMethod]
        public void PostDeleteAny()
        {
            var mockService = new Mock<IReservationsService>();
            mockService.Setup(m => m.GetReservation(It.IsAny<string>())).Returns(_mockData.reservations.First());
            var mockUserService = new Mock<IUsersService>();

            var controller = new ReservationsController(mockService.Object, mockUserService.Object);
            var result = controller.DeleteAnyPost("1") as RedirectToActionResult;
            var expectedRedirectValues = new RouteValueDictionary
            {
                { "message", "Rezerwacja została anulowana." },
                { "action", "Index" }
            };
            Assert.AreEqual(expectedRedirectValues.Values.ElementAt(1), result.ActionName);
            Assert.AreEqual(expectedRedirectValues.Values.ElementAt(0), result.RouteValues.ElementAt(0).Value);
        }
    }
}