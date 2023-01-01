

using Microsoft.AspNetCore.Identity;
using NarwianskiZakatek.Repositories;
using NarwianskiZakatek.ViewModels;

namespace NarwianskiZakatekUnitTests.services
{
    [TestClass]
    public class UsersServiceTests
    {
        public readonly MockData _mockData;

        public UsersServiceTests()
        {
            _mockData = new MockData();
        }

        [TestMethod]
        public void GetUserByUsername()
        {
            var mockContext = _mockData.Context.Object;
            var user = _mockData.appUsers.First();

            var service = new UsersService(mockContext);
            var result = service.GetUserByUsername(user.UserName);

            Assert.AreEqual(user, result);
        }

        [TestMethod]
        public void GetUserWarnings_ReturnsEmptyList()
        {
            var mockContext = _mockData.Context.Object;
            var user = _mockData.appUsers.First();

            var service = new UsersService(mockContext);
            var result = service.GetUserWarnings(user.UserName);

            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void GetUserWarnings_ReturnsNotEmptyList()
        {
            var mockContext = _mockData.Context.Object;
            var user = _mockData.appUsers.ElementAt(2);

            var service = new UsersService(mockContext);
            var result = service.GetUserWarnings(user.UserName);

            Assert.AreEqual(1, result.Count);
        }

        [TestMethod]
        public void GetUserIdsByRoleName_AdminCountEqual1()
        {
            var mockContext = _mockData.Context.Object;

            var service = new UsersService(mockContext);
            var result = service.GetUserIdsByRoleName("Admin");

            Assert.AreEqual(1, result.Count);
        }

        [TestMethod]
        public void GetUserIdsByRoleName_EmployeeCountEqual1()
        {
            var mockContext = _mockData.Context.Object;

            var service = new UsersService(mockContext);
            var result = service.GetUserIdsByRoleName("Employee");

            Assert.AreEqual(1, result.Count);
        }

        [TestMethod]
        public void GetUserIdsByRoleName_UserCountEqual2()
        {
            var mockContext = _mockData.Context.Object;

            var service = new UsersService(mockContext);
            var result = service.GetUserIdsByRoleName("User");

            Assert.AreEqual(2, result.Count);
        }

        [TestMethod]
        public void GetUserByParams_FilterByEmail()
        {
            var mockContext = _mockData.Context.Object;

            var service = new UsersService(mockContext);
            var result = service.GetUsersByParams("Admin@email.com", null, null, 1, 10);

            Assert.AreEqual(1, result.Result.Count);
        }

        [TestMethod]
        public void GetUserByParams_FilterByPhone()
        {
            var mockContext = _mockData.Context.Object;

            var service = new UsersService(mockContext);
            var result = service.GetUsersByParams(null, "111", null, 1, 10);

            Assert.AreEqual(2, result.Result.Count);
        }

        [TestMethod]
        public void GetUserByParams_FilterByRole()
        {
            var mockContext = _mockData.Context.Object;

            var service = new UsersService(mockContext);
            var result = service.GetUsersByParams(null, null, "KLIENT", 1, 10);

            Assert.AreEqual(2, result.Result.Count);
        }

        [TestMethod]
        public void AddToRole_Success()
        {
            var mockContext = _mockData.Context.Object;

            var service = new UsersService(mockContext);
            var result = service.AddToRole("Admin", "Employee@email.com");
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void AddToRole_UserDoesNotExist()
        {
            var mockContext = _mockData.Context.Object;

            var service = new UsersService(mockContext);
            var result = service.AddToRole("Admin", "Emloyee@email.com");
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void AddToRole_RoleDoesNotExist()
        {
            var mockContext = _mockData.Context.Object;

            var service = new UsersService(mockContext);
            var result = service.AddToRole("Admiin", "Employee@email.com");
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void RemoveFromRole_Success()
        {
            var mockContext = _mockData.Context.Object;

            var service = new UsersService(mockContext);
            var result = service.RemoveFromRole("Admin", "Admin@email.com");
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void RemoveFromRole_UserDoesNotExist()
        {
            var mockContext = _mockData.Context.Object;

            var service = new UsersService(mockContext);
            var result = service.RemoveFromRole("Admin", "Emloyee@email.com");
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void RemoveFromRole_RoleDoesNotExist()
        {
            var mockContext = _mockData.Context.Object;

            var service = new UsersService(mockContext);
            var result = service.RemoveFromRole("Admiin", "Employee@email.com");
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void SendWarning_UserDoesNotExist()
        {
            var mockContext = _mockData.Context.Object;
            var warningModel = new WarningViewModel()
            {
                UserName = "Emloyee@email.com",
                Message = "TestMessage"
            };
            var service = new UsersService(mockContext);
            var result = service.SendWarning(warningModel);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void SendWarning_Success()
        {
            var mockContext = _mockData.Context.Object;
            var warningModel = new WarningViewModel()
            {
                UserName = "Employee@email.com",
                Message = "TestMessage"
            };
            var service = new UsersService(mockContext);
            var result = service.SendWarning(warningModel);
            Assert.IsTrue(result);
        }
    }
}
