using Microsoft.EntityFrameworkCore;
using Moq;
using NarwianskiZakatek.Data;
using NarwianskiZakatek.Models;
using NarwianskiZakatek.Repositories;

namespace NarwianskiZakatekUnitTests
{
    public class DescriptionsServiceTests
    {
        public readonly Mock<ApplicationDbContext> _mockContext;

        public DescriptionsServiceTests()
        {
            _mockContext = new MockData().Context;
        }

        [TestMethod]
        public void Mock_DbContext_Test()
        {
            var service = new DescriptionsService(_mockContext.Object);
            var d = service.GetByTitle("Okolica");
            Assert.AreEqual("XXX", d.Content);
        }
    }
}
