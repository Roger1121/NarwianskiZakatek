using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NarwianskiZakatek.Data;
using NarwianskiZakatek.ViewModels;

namespace NarwianskiZakatek.Controllers
{
    public class ReservationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReservationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles ="User")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Date()
        {
            return View();
        }

        [HttpPost]
        public IActionResult New(DateTime beginDate, DateTime endDate)
        {
            ViewBag.RoomList = FindAvailableRooms(beginDate, endDate);
            return View();
        }

        [HttpPost]
        public IActionResult Confirm(AvailableRooms roomList)
        {
            //save
            return View();
        }

        private List<SelectListItem> FindAvailableRooms(DateTime beginDate, DateTime endDate)
        {
            var list = _context.Rooms.Select(x => new SelectListItem { Text = x.ToString(), Value = x.RoomId.ToString() }).ToList();

            return list;
        }
    }
}
