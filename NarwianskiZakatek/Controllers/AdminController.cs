using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NarwianskiZakatek.Data;
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
        public ActionResult Users(string? message)
        {
            ViewBag.Message = message;
            ViewBag.CurrentUser = HttpContext.User.Identity?.Name;
            return _context.Users != null ?
                        View(_context.Users.ToList()) :
                        Problem("Entity set 'ApplicationDbContext.Users'  is null.");
        }

        [Authorize(Roles = "Admin")]
        public ActionResult AddToRole(string role, string userName)
        {
            string? userId = _context.Users.Where(u => u.UserName == userName).FirstOrDefault()?.Id;
            string? roleId = _context.Roles.Where(u => u.Name == role).FirstOrDefault()?.Id;
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
            string? roleId = _context.Roles.Where(u => u.Name == role).FirstOrDefault()?.Id;
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
