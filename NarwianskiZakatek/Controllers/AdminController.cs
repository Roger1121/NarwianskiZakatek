 using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NarwianskiZakatek.Data;
using NarwianskiZakatek.Models;
using NarwianskiZakatek.Repositories;
using NarwianskiZakatek.Services;
using NarwianskiZakatek.ViewModels;

namespace NarwianskiZakatek.Controllers
{
    public class AdminController : Controller
    {
        private readonly IUsersService _service; 

        public AdminController(IUsersService service)
        {
            _service = service;
        }

        // GET: Users
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Users(string? message, int? pageNumber, string? email, string? phone, string? role)
        {
            var username = HttpContext.User.Identity?.Name;
            ViewBag.Warnings = _service.GetUserWarnings(username);

            ViewBag.Message = message;
            ViewBag.CurrentUser = HttpContext.User.Identity?.Name;
            ViewBag.Phone = phone;
            ViewBag.Email = email;
            ViewBag.Role = role;

            ViewBag.Admins = _service.GetUserIdsByRoleName("Admin");
            ViewBag.Employees = _service.GetUserIdsByRoleName("Employee");

            return View(await _service.GetUsersByParams(email, phone, role, pageNumber, 10));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddToRole(string role, string userName)
        {
            if (!_service.AddToRole(role, userName))
            {
                return RedirectToAction("Users", new { message = "Wystąpił błąd podczas nadawania uprawnień użytkownikowi." });
            }
            return RedirectToAction("Users", new { message = "Pomyślnie zmieniono rolę użytkownika." });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveFromRole(string role, string userName)
        {
            if (!_service.RemoveFromRole(role, userName))
            {
                return RedirectToAction("Users", new { message = "Wystąpił błąd podczas wycofywania uprawnień użytkownika." });
            }
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
        [ValidateAntiForgeryToken]
        public ActionResult SendWarning(WarningViewModel model)
        {
            _service.SendWarning(model);
            return RedirectToAction("Users", new { message = "Wysłano ostrzeżenie użytkownikowi." });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LockAccount(string userName)
        {
            _service.LockAccount(userName);
            return RedirectToAction("Users", new { message = "Konto użytkownika zostało zablokowane." });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UnlockAccount(string userName)
        {
            _service.UnlockAccount(userName);
            return RedirectToAction("Users", new { message = "Konto użytkownika zostało odblokowane." });
        }
    }
}
