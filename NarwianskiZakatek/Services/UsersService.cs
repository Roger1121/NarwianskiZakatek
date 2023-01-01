using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NarwianskiZakatek.Data;
using NarwianskiZakatek.Models;
using NarwianskiZakatek.Services;
using NarwianskiZakatek.ViewModels;
using System.Data;
using System.Web.Helpers;

namespace NarwianskiZakatek.Repositories
{
    public class UsersService : IUsersService
    {
        private readonly ApplicationDbContext _context;
        public UsersService(ApplicationDbContext context)
        {
            _context = context;
        }
        public AppUser GetUserByUsername(string username)
        {
            return _context.Users.Where(u => u.UserName == username).First();
        }
        public List<string> GetUserWarnings(string username)
        {
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
            return Warnings;
        }

        public List<string> GetUserIdsByRoleName(string role)
        {
            var roleId = _context.Roles.Where(r => r.NormalizedName == role.ToUpper()).First().Id;
            return _context.UserRoles.Where(u => u.RoleId == roleId).Select(u => u.UserId).ToList();
        }

        public async Task<PaginatedList<AppUser>> GetUsersByParams(string? email, string? phone, string? role, int? pageNumber, int? pageSize)
        {
            var employees = GetUserIdsByRoleName("Employee");
            var admins = GetUserIdsByRoleName("Admin");
            var users = _context.Users.OrderBy(u => u.NormalizedEmail).AsNoTracking();
            if (!string.IsNullOrEmpty(email))
                users = users.Where(u => u.NormalizedEmail.Contains(email.ToUpper()));
            if (!string.IsNullOrEmpty(phone))
                users = users.Where(u => u.PhoneNumber.Contains(phone));
            switch (role?.ToUpper())
            {
                case "ADMIN":
                    users = users.Where(u => admins.Contains(u.Id));
                    break;
                case "PRACOWNIK":
                    users = users.Where(u => employees.Contains(u.Id) && !admins.Contains(u.Id));
                    break;
                case "KLIENT":
                    users = users.Where(u => !employees.Contains(u.Id) && !admins.Contains(u.Id));
                    break;
            }
            return PaginatedList<AppUser>.Create(users, pageNumber ?? 1, pageSize ?? 10);
        }

        public bool AddToRole(string role, string userName)
        {
            string? userId = _context.Users.Where(u => u.NormalizedUserName == userName.ToUpper()).FirstOrDefault()?.Id;
            string? roleId = _context.Roles.Where(u => u.NormalizedName == role.ToUpper()).FirstOrDefault()?.Id;
            if (userId == null || roleId == null)
            {
                return false;
            }
            _context.UserRoles.Add(new IdentityUserRole<string>()
            {
                RoleId = roleId,
                UserId = userId
            });
            _context.SaveChanges();
            return true;
        }

        public bool RemoveFromRole(string role, string userName)
        {
            string? userId = _context.Users.Where(u => u.NormalizedUserName == userName.ToUpper()).FirstOrDefault()?.Id;
            string? roleId = _context.Roles.Where(u => u.NormalizedName == role.ToUpper()).FirstOrDefault()?.Id;
            if (userId == null || roleId == null)
            {
                return false;
            }
            _context.UserRoles.Remove(new IdentityUserRole<string>()
            {
                RoleId = roleId,
                UserId = userId
            });
            _context.SaveChanges();
            return true;
        }

        public bool SendWarning(WarningViewModel model)
        {
            var user = _context.Users.Where(u => u.NormalizedUserName == model.UserName.ToUpper()).FirstOrDefault();
            if (user == null)
            {
                return false;
            }
            _context.Warnings.Add(new Warning()
            {
                User = user,
                Message = model.Message
            });
            _context.SaveChanges();
            return true;
        }

        public void LockAccount(string userName)
        {
            var user = _context.Users.Where(u => u.NormalizedUserName == userName.ToUpper()).First();
            user.IsLocked = true;
            _context.SaveChanges();
        }
        public void UnlockAccount(string userName)
        {
            var user = _context.Users.Where(u => u.NormalizedUserName == userName.ToUpper()).First();
            user.IsLocked = false;
            _context.SaveChanges();
        }
    }
}
