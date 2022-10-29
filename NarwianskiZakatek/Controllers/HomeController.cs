using Microsoft.AspNetCore.Mvc;
using NarwianskiZakatek.Data;
using NarwianskiZakatek.Models;
using NarwianskiZakatek.ViewModels;
using System.Diagnostics;

namespace NarwianskiZakatek.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private Description _description;

        public HomeController(
            ILogger<HomeController> logger,
            ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Accomodation()
        {
            _description = _context.Descriptions.Where(d => d.Title == "Noclegi").First();
            return View(_description);
        }
        public IActionResult Catering()
        {
            _description = _context.Descriptions.Where(d => d.Title == "Restauracja").First();
            return View(_description);
        }

        public IActionResult Attractions()
        {
            _description = _context.Descriptions.Where(d => d.Title == "Atrakcje").First();
            return View(_description);
        }

        public IActionResult Neighborhood()
        {
            _description = _context.Descriptions.Where(d => d.Title == "Okolica").First();
            return View(_description);
        }

        public IActionResult About()
        {
            _description = _context.Descriptions.Where(d => d.Title == "O nas").First();
            return View(_description);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Edit(string header)
        {
            _description = _context.Descriptions.Where(d => d.Title == header).First();
            return View(new DescriptionViewModel()
            {
                PhotoUrl = _description.getFullPhotoPath(),
                Title = _description.Title,
                Content = _description.Content
            });
        }

        [HttpPost]
        public IActionResult Edit(DescriptionViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                Description description = _context.Descriptions.Where(d => d.Title == viewModel.Title).First();

                if (viewModel.File != null)
                {
                    string path = "wwwroot/graphics/descriptions/";
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(viewModel.File.FileName);

                    using (FileStream stream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
                    {
                        viewModel.File.CopyTo(stream);
                        if(description.PhotoUrl != null)
                        {
                            System.IO.File.Delete(description.getFullPhotoPath());
                        }
                        description.PhotoUrl = fileName;
                    }
                }

                description.Content = viewModel.Content;
                description.Title = viewModel.Title;
                _context.SaveChanges();

                switch (description.Title)
                {
                    case "Noclegi":
                        return RedirectToAction("Accomodation");
                    case "O nas":
                        return RedirectToAction("About");
                    case "Restauracja":
                        return RedirectToAction("Catering");
                    case "Atrakcje":
                        return RedirectToAction("Attractions");
                    case "Okolica":
                        return RedirectToAction("Neighborhood");
                }
            }
            return View();
        }
    }
}