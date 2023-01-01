using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Hosting;
using Moq;
using NarwianskiZakatek.Controllers;
using NarwianskiZakatek.Models;
using NarwianskiZakatek.Repositories;
using NarwianskiZakatek.Services;
using NarwianskiZakatek.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NarwianskiZakatekUnitTests
{
    [TestClass]
    public class PostsControllerTests
    {
        public MockData _mockData;
        public PostsControllerTests()
        {
            _mockData = new MockData();
        }

        [TestMethod]
        public void GetPosts_Admin()
        {
            var posts = new PaginatedList<Post>(_mockData.posts.GetRange(0, 10), 10, 0, 10);
            var mockService = new Mock<IPostsService>();
            mockService.Setup(x => x.GetPostsPage(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(posts);
            var mockUserService = new Mock<IUsersService>();

            var controller = new PostsController(mockService.Object, mockUserService.Object);
            var result = controller.Admin("msg", 10).Result as ViewResult;

            Assert.AreEqual("msg", controller.ViewBag.Message);
            Assert.AreEqual(posts, (PaginatedList<Post>)result.Model);
        }

        [TestMethod]
        public void GetDetails()
        {
            var mockService = new Mock<IPostsService>();
            mockService.Setup(x => x.GetPostDetails(It.IsAny<int>()))
                .Returns(_mockData.posts.First());
            var mockUserService = new Mock<IUsersService>();

            var controller = new PostsController(mockService.Object, mockUserService.Object);
            var result = controller.Details(10).Result as ViewResult;

            Assert.AreEqual(_mockData.posts.First(), (Post)result.Model);
        }

        [TestMethod]
        public void GetCreate()
        {
            var mockService = new Mock<IPostsService>();
            var mockUserService = new Mock<IUsersService>();

            var controller = new PostsController(mockService.Object, mockUserService.Object);
            var result = controller.Create() as ViewResult;

            Assert.IsNull(result.Model);
        }

        [TestMethod]
        public void PostCreate_ValidModel()
        {
            PostViewModel post = new PostViewModel()
            {
                Title = "test",
                Content = "test"
            };
            var mockService = new Mock<IPostsService>();
            var mockUserService = new Mock<IUsersService>();

            var controller = new PostsController(mockService.Object, mockUserService.Object);
            var result = controller.Create(post).Result as RedirectToActionResult;

            mockService.Verify(m => m.CreatePost(It.IsAny<PostViewModel>()));
            var expectedRedirectValues = new RouteValueDictionary
            {
                { "message", "Post został utworzony." },
                { "action", "Admin" }
            };
            Assert.AreEqual(expectedRedirectValues.Values.ElementAt(1), result.ActionName);
            Assert.AreEqual(expectedRedirectValues.Values.ElementAt(0), result.RouteValues.ElementAt(0).Value);
        }

        [TestMethod]
        public void PostCreate_InvalidModel()
        {
            PostViewModel post = new PostViewModel()
            {
                Title = "test",
                Content = "test"
            };
            var mockService = new Mock<IPostsService>();
            var mockUserService = new Mock<IUsersService>();

            var controller = new PostsController(mockService.Object, mockUserService.Object);
            controller.ModelState.AddModelError("test", "test");
            var result = controller.Create(post).Result as ViewResult;

            Assert.AreEqual(post, (PostViewModel)result.Model);
        }

        [TestMethod]
        public void GetEdit_NullId()
        {
            var mockService = new Mock<IPostsService>();
            var mockUserService = new Mock<IUsersService>();

            var controller = new PostsController(mockService.Object, mockUserService.Object);
            var result = controller.Edit(null).Result as NotFoundResult;

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetEdit_PostNotFound()
        {
            var mockService = new Mock<IPostsService>();
            mockService.Setup(m => m.GetPostDetails(It.IsAny<int>())).Returns((Post)null);
            var mockUserService = new Mock<IUsersService>();

            var controller = new PostsController(mockService.Object, mockUserService.Object);
            var result = controller.Edit(1).Result as NotFoundResult;

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetEdit_ValidationPassed()
        {
            PostViewModel postViewModel = new PostViewModel()
            {
                Title = "test",
                Content = "test"
            };
            Post post = new Post()
            {
                Title = "test",
                Content = "test"
            };
            var mockService = new Mock<IPostsService>();
            mockService.Setup(m => m.GetPostDetails(It.IsAny<int>())).Returns(post);
            var mockUserService = new Mock<IUsersService>();

            var controller = new PostsController(mockService.Object, mockUserService.Object);
            var result = controller.Edit(1).Result as ViewResult;

            Assert.AreEqual(postViewModel.Content, ((PostViewModel)result.Model).Content);
            Assert.AreEqual(postViewModel.Title, ((PostViewModel)result.Model).Title);
            Assert.AreEqual(postViewModel.PhotoUrl, ((PostViewModel)result.Model).PhotoUrl);
            Assert.AreEqual(postViewModel.PostId, ((PostViewModel)result.Model).PostId);
            Assert.AreEqual(postViewModel.DateCreated, ((PostViewModel)result.Model).DateCreated);
        }

        [TestMethod]
        public void PostEdit_WrongId()
        {
            PostViewModel postViewModel = new PostViewModel()
            {
                PostId = 1,
                Title = "test",
                Content = "test"
            };
            var mockService = new Mock<IPostsService>();
            var mockUserService = new Mock<IUsersService>();

            var controller = new PostsController(mockService.Object, mockUserService.Object);
            var result = controller.Edit(2, postViewModel).Result as NotFoundResult;

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void PostEdit_InvalidModel()
        {
            PostViewModel post = new PostViewModel()
            {
                PostId = 1,
                Title = "test",
                Content = "test"
            };
            var mockService = new Mock<IPostsService>();
            mockService.Setup(m => m.UpdatePost(post)).ReturnsAsync(false);
            var mockUserService = new Mock<IUsersService>();

            var controller = new PostsController(mockService.Object, mockUserService.Object);
            controller.ModelState.AddModelError("test", "test");
            var result = controller.Edit(1, post).Result as ViewResult;

            Assert.AreEqual(post, (PostViewModel)result.Model);
        }

        [TestMethod]
        public void PostEdit_ValidModel()
        {
            PostViewModel post = new PostViewModel()
            {
                PostId = 1,
                Title = "test",
                Content = "test"
            };
            var mockService = new Mock<IPostsService>();
            mockService.Setup(m => m.UpdatePost(post)).ReturnsAsync(true);
            var mockUserService = new Mock<IUsersService>();

            var controller = new PostsController(mockService.Object, mockUserService.Object);
            var result = controller.Edit(1, post).Result as RedirectToActionResult;

            var expectedRedirectValues = new RouteValueDictionary
            {
                { "message", "Post został zaktualizowany." },
                { "action", "Admin" }
            };
            Assert.AreEqual(expectedRedirectValues.Values.ElementAt(1), result.ActionName);
            Assert.AreEqual(expectedRedirectValues.Values.ElementAt(0), result.RouteValues.ElementAt(0).Value);
        }


        [TestMethod]
        public void GetDelete_NullId()
        {
            var mockService = new Mock<IPostsService>();
            var mockUserService = new Mock<IUsersService>();

            var controller = new PostsController(mockService.Object, mockUserService.Object);
            var result = controller.Delete(null).Result as NotFoundResult;

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetDelete_PostNotFound()
        {
            var mockService = new Mock<IPostsService>();
            mockService.Setup(m => m.GetPostDetails(It.IsAny<int>())).Returns((Post)null);
            var mockUserService = new Mock<IUsersService>();

            var controller = new PostsController(mockService.Object, mockUserService.Object);
            var result = controller.Delete(1).Result as NotFoundResult;

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetDelete_ValidationPassed()
        {
            Post post = new Post()
            {
                Title = "test",
                Content = "test"
            };
            var mockService = new Mock<IPostsService>();
            mockService.Setup(m => m.GetPostDetails(It.IsAny<int>())).Returns(post);
            var mockUserService = new Mock<IUsersService>();

            var controller = new PostsController(mockService.Object, mockUserService.Object);
            var result = controller.Delete(1).Result as ViewResult;

            Assert.AreEqual(post.Content, ((Post)result.Model).Content);
            Assert.AreEqual(post.Title, ((Post)result.Model).Title);
            Assert.AreEqual(post.PhotoUrl, ((Post)result.Model).PhotoUrl);
            Assert.AreEqual(post.PostId, ((Post)result.Model).PostId);
            Assert.AreEqual(post.DateCreated, ((Post)result.Model).DateCreated);
        }

        [TestMethod]
        public void PostDelete() 
        {
            var mockService = new Mock<IPostsService>();
            var mockUserService = new Mock<IUsersService>();

            var controller = new PostsController(mockService.Object, mockUserService.Object);
            var result = controller.DeleteConfirmed(1).Result as RedirectToActionResult;

            var expectedRedirectValues = new RouteValueDictionary
            {
                { "message", "Post został usunięty." },
                { "action", "Admin" }
            };
            Assert.AreEqual(expectedRedirectValues.Values.ElementAt(1), result.ActionName);
            Assert.AreEqual(expectedRedirectValues.Values.ElementAt(0), result.RouteValues.ElementAt(0).Value);
        }
    }
}
