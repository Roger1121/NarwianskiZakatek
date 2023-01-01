using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NarwianskiZakatek.Data;
using NarwianskiZakatek.Models;
using NarwianskiZakatek.Repositories;
using NarwianskiZakatek.Services;
using NarwianskiZakatek.ViewModels;
using System.Data;

namespace NarwianskiZakatek.Controllers
{
    public class PostsController : Controller
    {
        private readonly IPostsService _service;
        private readonly IUsersService _userService;

        public PostsController(IPostsService service, IUsersService userService)
        {
            _service = service;
            _userService = userService;
        }

        // GET: Posts
        public async Task<IActionResult> Index(int? pageNumber)
        {
            var username = HttpContext.User.Identity?.Name;
            ViewBag.Warnings = _userService.GetUserWarnings(username);

            int pageSize = 10;
            return View(_service.GetPostsPage(pageNumber ?? 1, pageSize));
        }

        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> Admin(string? message, int? pageNumber)
        {
            ViewBag.Message = message;
            int pageSize = 10;
            return View(_service.GetPostsPage(pageNumber ?? 1, pageSize));
        }

        // GET: Posts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = _service.GetPostDetails((int)id);
            return post == null ? NotFound() : View(post);
        }

        [Authorize(Roles = "Admin,Employee")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Posts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin,Employee")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,DateCreated,Content,File")] PostViewModel post)
        {
            if (ModelState.IsValid)
            {
                _service.CreatePost(post);
                return RedirectToAction("Admin", new { message = "Post został utworzony." });
            }
            return View(post);
        }

        // GET: Posts/Edit/5
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = _service.GetPostDetails((int)id);
            return post == null ? NotFound() : View(new PostViewModel()
            {
                PhotoUrl = post.getFullPhotoPath(),
                PostId = post.PostId,
                Title = post.Title,
                Content = post.Content,
                DateCreated = post.DateCreated
            });
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> Edit(int id, PostViewModel editedPost)
        {
            if (id != editedPost.PostId)
            {
                return NotFound();
            }

            if (ModelState.IsValid && await _service.UpdatePost(editedPost))
            {
                return RedirectToAction("Admin", new { message = "Post został zaktualizowany." });
            }
            return View(editedPost);
        }

        // GET: Posts/Delete/5
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = _service.GetPostDetails((int)id);
            return post == null ? NotFound() : View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            _service.Delete(id);
            return RedirectToAction("Admin", new { message = "Post został usunięty." });
        }
    }
}
