using NarwianskiZakatek.Models;
using NarwianskiZakatek.ViewModels;

namespace NarwianskiZakatek.Repositories
{
    public interface IUsersService
    {
        bool AddToRole(string role, string userName);
        AppUser GetUserByUsername(string username);
        List<string> GetUserIdsByRoleName(string role);
        Task<List<AppUser>> GetUsersByParams(string? email, string? phone, string? role, int? pageNumber, int? pageSize);
        List<string> GetUserWarnings(string username);
        void LockAccount(string userName);
        bool RemoveFromRole(string role, string userName);
        bool SendWarning(WarningViewModel model);
        void UnlockAccount(string userName);
    }
}