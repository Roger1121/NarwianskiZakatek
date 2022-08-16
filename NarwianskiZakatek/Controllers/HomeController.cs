using Microsoft.AspNetCore.Mvc;
using NarwianskiZakatek.Models;
using System.Diagnostics;

namespace NarwianskiZakatek.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Accomodation()
        {
            return View();
        }
        public IActionResult Catering()
        {
            return View();
        }

        public IActionResult Attractions()
        {
            return View();
        }

        public IActionResult Neighborhood()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}