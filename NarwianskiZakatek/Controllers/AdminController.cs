using Microsoft.AspNetCore.Authorization;
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
    }
}
