using NarwianskiZakatek.Repositories;
using NarwianskiZakatek.ViewModels;

namespace NarwianskiZakatekUnitTests.services
{
    [TestClass]
    public class PostsServiceTests
    {
        public readonly MockData _mockData;

        public PostsServiceTests()
        {
            _mockData = new MockData();
        }

        [TestMethod]
        public void GetPostsPage()
        {
            var service = new PostsService(_mockData.Context.Object);
            var posts = service.GetPostsPage(2, 10);
            Assert.AreEqual(1, posts.Count);
        }

        [TestMethod]
        public void GetPostDetails()
        {
            var service = new PostsService(_mockData.Context.Object);
            var post = service.GetPostDetails(1);
            Assert.AreEqual(_mockData.posts.First(), post);
        }

        [TestMethod]
        public void Update()
        {
            var service = new PostsService(_mockData.Context.Object);
            var viewModel = new PostViewModel()
            {
                PostId = 1,
                Title = "Post 1",
                Content = "New content"
            };

            var result = service.UpdatePost(viewModel).Result;
            Assert.IsTrue(result);
            Assert.AreEqual(viewModel.Content, _mockData.posts.Where(d => d.Title == "Post 1").First().Content);
        }
    }
}
