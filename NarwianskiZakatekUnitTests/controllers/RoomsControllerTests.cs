using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Moq;
using NarwianskiZakatek.Controllers;
using NarwianskiZakatek.Data;
using NarwianskiZakatek.Models;
using NarwianskiZakatek.Repositories;

namespace NarwianskiZakatekUnitTests.controllers
{
    [TestClass]
    public class RoomsControllerTests
    {
        [TestMethod]
        public void GetIndex_ReturnsView()
        {
            var mockRoomService = new Mock<IRoomsService>();
            mockRoomService.Setup(m => m.GetRooms()).Returns(new List<Room>());

            var controller = new RoomsController(mockRoomService.Object);
            var result = controller.Index("").Result as ViewResult;
            var rooms = (List<Room>)result.Model;
            Assert.AreEqual(0, rooms.Count);
        }

        [TestMethod]
        public void GetCreate_ReturnsEmptyView()
        {
            var mockRoomService = new Mock<IRoomsService>();

            var controller = new RoomsController(mockRoomService.Object);
            var result = controller.Create() as ViewResult;
            Assert.IsNull(result.Model);
        }

        [TestMethod]
        public void PostCreate_ValidationFails()
        {
            var room = new Room()
            {
                Price = -250
            };
            var mockRoomService = new Mock<IRoomsService>();

            var controller = new RoomsController(mockRoomService.Object);
            controller.ModelState.AddModelError("test", "test");
            var result = controller.Create(room).Result as ViewResult;
            Assert.AreEqual(room, (Room)result.Model);
        }

        [TestMethod]
        public void PostCreate_ValidationPassed()
        {
            var room = new Room()
            {
                RoomNumber = "111",
                RoomCapacity = 4,
                Price = 250,
                IsActive = true
            };
            var mockRoomService = new Mock<IRoomsService>();
            mockRoomService.Setup(m => m.AddRoom(room));

            var controller = new RoomsController(mockRoomService.Object);
            var result = controller.Create(room).Result as RedirectToActionResult;
            Assert.AreEqual("Index", result.ActionName);
        }

        [TestMethod]
        public void GetEdit_ReturnsView()
        {
            var room = new Room()
            {
                RoomId = 1,
                RoomNumber = "111",
                RoomCapacity = 4,
                Price = 250,
                IsActive = true
            };
            var mockRoomService = new Mock<IRoomsService>();
            mockRoomService.Setup(m => m.Get(It.IsAny<int>())).ReturnsAsync(room);
            var controller = new RoomsController(mockRoomService.Object);
            var result = controller.Edit(1).Result as ViewResult;
            Assert.AreEqual(room, (Room)result.Model);
        }

        [TestMethod]
        public void PostEdit_ValidationFails()
        {
            var room = new Room()
            {
                RoomId = 1
            };
            var mockRoomService = new Mock<IRoomsService>();

            var controller = new RoomsController(mockRoomService.Object);
            controller.ModelState.AddModelError("test", "test");
            var result = controller.Edit(1, room).Result as ViewResult;
            Assert.AreEqual(room, (Room)result.Model);
        }

        [TestMethod]
        public void PostEdit_WrongRoomId()
        {
            var room = new Room();
            var mockRoomService = new Mock<IRoomsService>();

            var controller = new RoomsController(mockRoomService.Object);
            controller.ModelState.AddModelError("test", "test");
            var result = controller.Edit(1, room).Result as NotFoundResult;
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void PostEdit_ValidationPassed()
        {
            var room = new Room()
            {
                RoomId = 1,
                RoomNumber = "111",
                RoomCapacity = 4,
                Price = 250,
                IsActive = true
            };
            var mockRoomService = new Mock<IRoomsService>();
            mockRoomService.Setup(m => m.AddRoom(room));

            var controller = new RoomsController(mockRoomService.Object);
            var result = controller.Edit(1, room).Result as RedirectToActionResult;
            Assert.AreEqual("Index", result.ActionName);
        }

        [TestMethod]
        public void GetDelete_ReturnsView()
        {
            var room = new Room()
            {
                RoomId = 1,
                RoomNumber = "111",
                RoomCapacity = 4,
                Price = 250,
                IsActive = true
            };
            var mockRoomService = new Mock<IRoomsService>();
            mockRoomService.Setup(m => m.Get(It.IsAny<int>())).ReturnsAsync(room);
            var controller = new RoomsController(mockRoomService.Object);
            var result = controller.Edit(1).Result as ViewResult;
            Assert.AreEqual(room, (Room)result.Model);
        }

        [TestMethod]
        public void PostDelete_ValidationPassed()
        {
            var room = new Room()
            {
                RoomNumber = "111",
                RoomId = 1
            };
            var mockRoomService = new Mock<IRoomsService>();
            mockRoomService.Setup(m => m.Get(It.IsAny<int>())).ReturnsAsync(room);
            mockRoomService.Setup(m => m.Delete(It.IsAny<Room>())).ReturnsAsync(true);

            var expectedRedirectValues = new RouteValueDictionary
            {
                { "message", "Pomyślnie usunięto pokój o numerze " + room.RoomNumber + "." },
                { "action", "Index" },
            };

            var controller = new RoomsController(mockRoomService.Object);
            var result = controller.DeleteConfirmed(1).Result as RedirectToActionResult;
            Assert.AreEqual(expectedRedirectValues.Values.ElementAt(1), result.ActionName);
            Assert.AreEqual(expectedRedirectValues.Values.ElementAt(0), result.RouteValues.ElementAt(0).Value);
        }

        [TestMethod]
        public void PostDelete_ValidationFailed()
        {
            var room = new Room()
            {
                RoomId = 1
            };
            var mockRoomService = new Mock<IRoomsService>();
            mockRoomService.Setup(m => m.Delete(It.IsAny<Room>())).ReturnsAsync(false);

            var expectedRedirectValues = new RouteValueDictionary
            {
                { "message", "Pokój nie został usunięty, gdyż istnieją w systemie powiązane z nim rezerwacje." +
                " Jeżeli chcesz wycofać pokój z użytku, zmień jego dostępność w oknie edycji pokoju." },
                { "action", "Index" },
            };

            var controller = new RoomsController(mockRoomService.Object);
            var result = controller.DeleteConfirmed(1).Result as RedirectToActionResult;
            Assert.AreEqual(expectedRedirectValues.Values.ElementAt(1), result.ActionName);
            Assert.AreEqual(expectedRedirectValues.Values.ElementAt(0), result.RouteValues.ElementAt(0).Value);
        }
    }
}
