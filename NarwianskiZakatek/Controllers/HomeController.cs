using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NarwianskiZakatek.Data;
using NarwianskiZakatek.Models;
using NarwianskiZakatek.Repositories;
using NarwianskiZakatek.ViewModels;
using System.Data;
using System.Diagnostics;

namespace NarwianskiZakatek.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IDescriptionsService _service;

        public HomeController(
            ILogger<HomeController> logger,
            IDescriptionsService service)
        {
            _logger = logger;
            _service = service;
        }

        public IActionResult Accomodation()
        {
            return View(_service.GetByTitle("Noclegi"));
        }
        public IActionResult Catering()
        {
            return View(_service.GetByTitle("Restauracja"));
        }

        public IActionResult Attractions()
        {
            return View(_service.GetByTitle("Atrakcje"));
        }

        public IActionResult Neighborhood()
        {
            return View(_service.GetByTitle("Okolica"));
        }

        public IActionResult About()
        {
            return View(_service.GetByTitle("O nas"));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Authorize(Roles = "Admin,Employee")]
        public IActionResult Edit(string header)
        {
            var description = _service.GetByTitle(header);
            return View(new DescriptionViewModel()
            {
                PhotoUrl = description.getFullPhotoPath(),
                Title = description.Title,
                Content = description.Content
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Employee")]
        public IActionResult Edit(DescriptionViewModel viewModel)
        {
            Description description = _service.GetByTitle(viewModel.Title);
            if (ModelState.IsValid)
            {
                _service.Update(viewModel);
                description = _service.GetByTitle(viewModel.Title);
                return View(new DescriptionViewModel()
                {
                    PhotoUrl = description.getFullPhotoPath(),
                    Title = description.Title,
                    Content = description.Content
                }); 
            }
            return View(new DescriptionViewModel()
            {
                PhotoUrl = description.getFullPhotoPath(),
                Title = description.Title,
                Content = description.Content
            });
        }
    }
}