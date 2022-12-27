using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NarwianskiZakatek.Data;
using NarwianskiZakatek.Models;
using NarwianskiZakatek.Services;
using NarwianskiZakatek.ViewModels;
using System.Data;

namespace NarwianskiZakatek.Controllers
{
    public class PostsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public PostsController(UserManager<AppUser> userManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        // GET: Posts
        public async Task<IActionResult> Index(int? pageNumber)
        {
            var username = HttpContext.User.Identity?.Name;
            var Warnings = new List<string>();
            if (username != null)
            {
                var user = _context.Users.Where(u => u.UserName == username).First();
                var warnings = _context.Warnings?.Where(w => w.UserId == user.Id && w.WasDisplayed == false).ToList();
                if (warnings != null)
                {
                    foreach (var warning in warnings)
                    {
                        warning.WasDisplayed = true;
                        Warnings.Add(warning.Message);
                    }
                    _context.SaveChanges();
                }
            }
            ViewBag.Warnings = Warnings;

            int pageSize = 10;
            return _context.Posts != null ?
                View(await PaginatedList<Post>.CreateAsync(_context.Posts.OrderByDescending(p => p.DateCreated).AsNoTracking(), pageNumber ?? 1, pageSize)) :
                          Problem("Entity set 'ApplicationDbContext.Posts'  is null.");
        }

        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> Admin(string? message, int? pageNumber)
        {
            ViewBag.Message = message;
            int pageSize = 10;
            return _context.Posts != null ?
                View(await PaginatedList<Post>.CreateAsync(_context.Posts.OrderByDescending(p => p.DateCreated).AsNoTracking(), pageNumber ?? 1, pageSize)) :
                          Problem("Entity set 'ApplicationDbContext.Posts'  is null.");
        }

        // GET: Posts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Posts == null)
            {
                return NotFound();
            }

            var post = await _context.Posts
                .FirstOrDefaultAsync(m => m.PostId == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
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
                Post newPost = new Post();
                if (post.File != null)
                {
                    string path = "wwwroot/graphics/posts/" + DateTime.Now.Year;
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(post.File.FileName).ToLower();

                    using (FileStream stream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
                    {
                        post.File.CopyTo(stream);
                        newPost.PhotoUrl = fileName;
                    }
                }

                newPost.Content = post.Content;
                newPost.Title = post.Title;
                newPost.DateCreated = DateTime.Now;
                _context.Add(newPost);
                await _context.SaveChangesAsync();
                return RedirectToAction("Admin", new { message = "Post został utworzony." });
            }
            return View(post);
        }

        // GET: Posts/Edit/5
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Posts == null)
            {
                return NotFound();
            }

            var post = await _context.Posts.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }
            return View(new PostViewModel()
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

            if (ModelState.IsValid)
            {
                try
                {
                    Post post = _context.Posts.Where(p => p.PostId == editedPost.PostId).First();
                    post.Title = editedPost.Title;
                    post.Content = editedPost.Content;
                    if (editedPost.File != null)
                    {
                        string path = "wwwroot/graphics/posts/" + DateTime.Now.Year;
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(editedPost.File.FileName).ToLower();

                        using (FileStream stream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
                        {
                            editedPost.File.CopyTo(stream);
                            if (post.PhotoUrl != null)
                            {
                                System.IO.File.Delete("wwwroot" + post.getFullPhotoPath());
                            }
                            post.PhotoUrl = fileName;
                        }
                    }
                    _context.Update(post);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostExists(editedPost.PostId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Admin", new { message = "Post został zaktualizowany." });
            }
            return View(editedPost);
        }

        // GET: Posts/Delete/5
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Posts == null)
            {
                return NotFound();
            }

            var post = await _context.Posts
                .FirstOrDefaultAsync(m => m.PostId == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Posts == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Posts'  is null.");
            }
            var post = await _context.Posts.FindAsync(id);
            if (post != null)
            {
                if (post.PhotoUrl != null)
                {
                    System.IO.File.Delete("wwwroot" + post.getFullPhotoPath());
                }
                _context.Posts.Remove(post);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Admin", new { message = "Post został usunięty." });
        }

        private bool PostExists(int id)
        {
            return (_context.Posts?.Any(e => e.PostId == id)).GetValueOrDefault();
        }
    }
}
