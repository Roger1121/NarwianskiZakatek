using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NarwianskiZakatek.Models;

namespace NarwianskiZakatek.Controllers
{
    public class ReservationsController : Controller
    {
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult New()
        {
            return View();
        }

        [HttpPost]
        public IActionResult New(Reservation reservation)
        {
            return View();
        }
    }
}
