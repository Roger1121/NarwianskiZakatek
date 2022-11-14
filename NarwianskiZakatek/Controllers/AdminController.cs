 using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NarwianskiZakatek.Data;
using NarwianskiZakatek.Models;
using NarwianskiZakatek.Services;
using NarwianskiZakatek.ViewModels;

namespace NarwianskiZakatek.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        // GET: Users
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Users(string? message, int? pageNumber, string? email, string? phone, string? role)
        {
            ViewBag.Message = message;
            ViewBag.CurrentUser = HttpContext.User.Identity?.Name;
            ViewBag.Phone = phone;
            ViewBag.Email = email;
            ViewBag.Role = role;

            var adminId = _context.Roles.Where(r => r.NormalizedName == "ADMIN").First().Id;
            var employeeId = _context.Roles.Where(r => r.NormalizedName == "EMPLOYEE").First().Id;

            var admins = _context.UserRoles.Where(u => u.RoleId == adminId).Select(u => u.UserId).ToList();
            var employees = _context.UserRoles.Where(u => u.RoleId == employeeId).Select(u => u.UserId).ToList();

            ViewBag.Admins = admins;
            ViewBag.Employees = employees;
            int pageSize = 3;

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
                    users = users.Where(u => !employees.Contains(u.Id));
                    break;
            }

            return View(await PaginatedList<AppUser>.CreateAsync(users, pageNumber ?? 1, pageSize));
        }

        [Authorize(Roles = "Admin")]
        public ActionResult AddToRole(string role, string userName)
        {
            string? userId = _context.Users.Where(u => u.UserName == userName).FirstOrDefault()?.Id;
            string? roleId = _context.Roles.Where(u => u.NormalizedName == role).FirstOrDefault()?.Id;
            if (userId == null || roleId == null)
            {
                return RedirectToAction("Users", new { message = "Wystąpił błąd podczas nadawania uprawnień użytkownikowi." });
            }
            _context.UserRoles.Add(new IdentityUserRole<string>()
            {
                RoleId = roleId,
                UserId = userId
            });
            _context.SaveChanges();
            return RedirectToAction("Users", new { message = "Pomyślnie zmieniono rolę użytkownika." });
        }

        [Authorize(Roles = "Admin")]
        public ActionResult RemoveFromRole(string role, string userName)
        {
            string? userId = _context.Users.Where(u => u.UserName == userName).FirstOrDefault()?.Id;
            string? roleId = _context.Roles.Where(u => u.NormalizedName == role).FirstOrDefault()?.Id;
            if (userId == null || roleId == null)
            {
                return RedirectToAction("Users", new { message = "Wystąpił błąd podczas wycofywania uprawnień użytkownika." });
            }
            _context.UserRoles.Remove(new IdentityUserRole<string>()
            {
                RoleId = roleId,
                UserId = userId
            });
            _context.SaveChanges();
            return RedirectToAction("Users", new { message = "Pomyślnie zmieniono rolę użytkownika." });
        }

        [Authorize(Roles = "Admin")]
        public ActionResult SendWarning(string userName)
        {
            var model = new WarningViewModel()
            {
                UserName = userName
            };

            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult SendWarning(WarningViewModel model)
        {
            var userId = _context.Users.Where(u => u.UserName == model.UserName).First().Id;
            if (_context.Warnings == null)
            {
                return RedirectToAction("Users", new { message = "Wystąpił błąd podczas wysyłania ostrzeżenia." });
            }
            _context.Warnings.Add(new Models.Warning()
            {
                UserId = userId,
                Message = model.Message
            });
            _context.SaveChanges();
            return RedirectToAction("Users", new { message = "Wysłano ostrzeżenie użytkownikowi." });
        }

        [Authorize(Roles = "Admin")]
        public ActionResult LockAccount(string userName)
        {
            var user = _context.Users.Where(u => u.UserName == userName).First();
            user.IsLocked = true;
            _context.SaveChanges();
            return RedirectToAction("Users", new { message = "Konto użytkownika zostało zablokowane." });
        }

        [Authorize(Roles = "Admin")]
        public ActionResult UnlockAccount(string userName)
        {
            var user = _context.Users.Where(u => u.UserName == userName).First();
            user.IsLocked = false;
            _context.SaveChanges();
            return RedirectToAction("Users", new { message = "Konto użytkownika zostało odblokowane." });
        }
    }
}
