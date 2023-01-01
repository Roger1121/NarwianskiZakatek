using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NarwianskiZakatek.Data;
using NarwianskiZakatek.Models;
using NarwianskiZakatek.Services;
using NarwianskiZakatek.ViewModels;
using System.Drawing.Printing;

namespace NarwianskiZakatek.Repositories
{
    public class PostsService : IPostsService
    {
        private readonly ApplicationDbContext _context;

        public PostsService(ApplicationDbContext context)
        {
            _context = context;
        }

        public PaginatedList<Post> GetPostsPage(int pageNumber, int pageSize)
        {
            return PaginatedList<Post>.Create(_context.Posts.OrderByDescending(p => p.DateCreated).AsNoTracking(), pageNumber, pageSize);
        }

        public Post? GetPostDetails(int id)
        {
            return _context.Posts
                .FirstOrDefault(m => m.PostId == id) ?? null;
        }

        public async Task CreatePost(PostViewModel post)
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
        }

        public async Task<bool> UpdatePost(PostViewModel editedPost)
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
                else if (post.PhotoUrl != null)
                {
                    System.IO.File.Delete("wwwroot" + post.getFullPhotoPath());
                    post.PhotoUrl = null;
                }
                _context.Update(post);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
        }

        public async Task Delete(int id)
        {
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
        }
    }
}
