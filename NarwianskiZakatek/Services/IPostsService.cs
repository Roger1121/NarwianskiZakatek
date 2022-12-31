using NarwianskiZakatek.Models;
using NarwianskiZakatek.Services;
using NarwianskiZakatek.ViewModels;

namespace NarwianskiZakatek.Repositories
{
    public interface IPostsService
    {
        Task CreatePost(PostViewModel post);
        Task Delete(int id);
        Task<Post?> GetPostDetails(int id);
        Task<PaginatedList<Post>> GetPostsPage(int pageNumber, int pageSize);
        Task<bool> UpdatePost(PostViewModel editedPost);
    }
}