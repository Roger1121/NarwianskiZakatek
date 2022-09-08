using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NarwianskiZakatek.Data;

namespace NarwianskiZakatek.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Users
        [Authorize(Roles= "Admin")]
        public ActionResult Users()
        {
            return _context.Users != null ?
                        View(_context.Users.ToList()) :
                        Problem("Entity set 'ApplicationDbContext.Users'  is null.");
        }

        [Authorize(Roles = "Admin")]
        public ActionResult AddToRole(string role, string userName)
        {
            string userId = _context.Users.Where(u => u.UserName == userName).FirstOrDefault().Id;
            string roleId = _context.Roles.Where(u => u.Name == role).FirstOrDefault().Id;
            _context.UserRoles.Add(new IdentityUserRole<string>()
            {
                RoleId = roleId,
                UserId = userId
            });
            _context.SaveChanges();
            return RedirectToAction("Users");
        }

        [Authorize(Roles = "Admin")]
        public ActionResult RemoveFromRole(string role, string userName)
        {
            string userId = _context.Users.Where(u => u.UserName == userName).FirstOrDefault().Id;
            string roleId = _context.Roles.Where(u => u.Name == role).FirstOrDefault().Id;
            _context.UserRoles.Remove(new IdentityUserRole<string>()
            {
                RoleId = roleId,
                UserId = userId
            });
            _context.SaveChanges();
            return RedirectToAction("Users");
        }
    }
}
