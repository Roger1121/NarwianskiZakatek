using Microsoft.EntityFrameworkCore;
using Moq;
using NarwianskiZakatek.Data;
using NarwianskiZakatek.Models;
using NarwianskiZakatek.Repositories;
using NarwianskiZakatek.ViewModels;

namespace NarwianskiZakatekUnitTests.services
{
    [TestClass]
    public class DescriptionsServiceTests
    {
        public readonly MockData _mockData;

        public DescriptionsServiceTests()
        {
            _mockData = new MockData();
        }

        [TestMethod]
        public void GetByTitle()
        {
            var service = new DescriptionsService(_mockData.Context.Object);
            var d = service.GetByTitle("Test description 3");
            Assert.AreEqual(_mockData.descriptions.Where(d => d.Title == "Test description 3").First(), d);
        }

        [TestMethod]
        public void Update()
        {
            var service = new DescriptionsService(_mockData.Context.Object);
            var viewModel = new DescriptionViewModel()
            {
                Title = "Test description 2",
                Content = "New content"
            };

            service.Update(viewModel);

            Assert.AreEqual(viewModel.Content, _mockData.descriptions.Where(d => d.Title == "Test description 2").First().Content);
        }
    }
}
