using Moq;
using NarwianskiZakatek.Models;
using NarwianskiZakatek.Repositories;
using NarwianskiZakatek.Services;

namespace NarwianskiZakatekUnitTests
{
    [TestClass]
    public class RoomsServiceTests
    {
        public readonly MockData _mockData;

        public RoomsServiceTests()
        {
            _mockData = new MockData();
        }

        [TestMethod]
        public void GetRooms()
        {
            var mockSender = new Mock<IEmailService>().Object;
            var service = new RoomsService(_mockData.Context.Object, mockSender);

            var result = service.GetRooms();

            _mockData.rooms.ForEach(r => Assert.IsTrue(result.Contains(r)));
        }

        [TestMethod]
        public void UpdateRoom_IsActiveSwitchedToFalse()
        {
            var room = new Room(_mockData.rooms.First());
            room.IsActive = false;

            var mockSender = new Mock<IEmailService>().Object;
            var service = new RoomsService(_mockData.Context.Object, mockSender);

            var result = service.UpdateRoom(room).Result;

            Assert.AreEqual("Pokój został zaktualizowany. Pokój został wyłączony z użycia, a wszystkie nadchodzące rezerwacje tego pokoju zostały odwołane.", result);
        }

        [TestMethod]
        public void UpdateRoom_IsActiveNotChanged()
        {
            var room = new Room(_mockData.rooms.First());
            room.Price = 200;

            var mockSender = new Mock<IEmailService>().Object;
            var service = new RoomsService(_mockData.Context.Object, mockSender);

            var result = service.UpdateRoom(room).Result;

            Assert.AreEqual("Pokój został zaktualizowany.", result);
        }

        [TestMethod]
        public void Delete_ShouldFail()
        {
            var room = _mockData.rooms.First();
            room.ReservedRooms = new List<ReservedRoom>();
            room.ReservedRooms.Add(new ReservedRoom());

            var mockSender = new Mock<IEmailService>().Object;
            var service = new RoomsService(_mockData.Context.Object, mockSender);

            var result = service.Delete(room).Result;
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Delete_ShouldSucceed()
        {
            var room = _mockData.rooms.First();
            room.ReservedRooms = new List<ReservedRoom>();

            var mockSender = new Mock<IEmailService>().Object;
            var service = new RoomsService(_mockData.Context.Object, mockSender);

            var result = service.Delete(room).Result;
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void RoomExists_ReturnsTrue()
        {
            var mockSender = new Mock<IEmailService>().Object;
            var service = new RoomsService(_mockData.Context.Object, mockSender);

            var result = service.RoomExists(1);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void RoomExists_ReturnsFalse()
        {
            var mockSender = new Mock<IEmailService>().Object;
            var service = new RoomsService(_mockData.Context.Object, mockSender);

            var result = service.RoomExists(100);

            Assert.IsFalse(result);
        }
    }
}
