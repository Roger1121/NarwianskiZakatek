using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NarwianskiZakatek.Data;
using NarwianskiZakatek.Models;

namespace NarwianskiZakatek.Controllers
{
    public class ReservationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReservationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult New()
        {
            ViewBag.RoomsList = new LinkedList<SelectListItem>();
            return View();
        }

        [HttpPost]
        public IActionResult New(Reservation reservation)
        {
            return View();
        }

        public JsonResult FindAvailableRooms(/*DateTime beginDate, DateTime endDate*/)
        {
            var list = _context.Rooms.ToList();

            return Json(list);
        }
    }
}
