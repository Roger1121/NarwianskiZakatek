using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NarwianskiZakatek.Data;
using NarwianskiZakatek.Models;
using NarwianskiZakatek.ViewModels;
using System.Data;
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
            if(_context.Descriptions != null)
                _description = _context.Descriptions.Where(d => d.Title == "Noclegi").First();
            return View(_description);
        }
        public IActionResult Catering()
        {
            if (_context.Descriptions != null)
                _description = _context.Descriptions.Where(d => d.Title == "Restauracja").First();
            return View(_description);
        }

        public IActionResult Attractions()
        {
            if (_context.Descriptions != null)
                _description = _context.Descriptions.Where(d => d.Title == "Atrakcje").First();
            return View(_description);
        }

        public IActionResult Neighborhood()
        {
            if (_context.Descriptions != null)
                _description = _context.Descriptions.Where(d => d.Title == "Okolica").First();
            return View(_description);
        }

        public IActionResult About()
        {
            if (_context.Descriptions != null)
                _description = _context.Descriptions.Where(d => d.Title == "O nas").First();
            return View(_description);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Authorize(Roles = "Admin,Employee")]
        public IActionResult Edit(string header)
        {
            if (_context.Descriptions != null)
                _description = _context.Descriptions.Where(d => d.Title == header).First();
            return View(new DescriptionViewModel()
            {
                PhotoUrl = _description.getFullPhotoPath(),
                Title = _description.Title,
                Content = _description.Content
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Employee")]
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
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(viewModel.File.FileName).ToLower();

                    using (FileStream stream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
                    {
                        viewModel.File.CopyTo(stream);
                        if(description.PhotoUrl != null)
                        {
                            System.IO.File.Delete("wwwroot" + description.getFullPhotoPath());
                        }
                        description.PhotoUrl = fileName;
                    }
                }
                else if (description.PhotoUrl != null)
                {
                    System.IO.File.Delete("wwwroot" + description.getFullPhotoPath());
                    description.PhotoUrl = null;
                }

                description.Content = viewModel.Content;
                description.Title = viewModel.Title;
                _context.SaveChanges();

                description = _context.Descriptions.Where(d => d.Title == viewModel.Title).First();
                return View(new DescriptionViewModel()
                {
                    PhotoUrl = description.getFullPhotoPath(),
                    Title = description.Title,
                    Content = description.Content
                }); 
            }
            return View();
        }
    }
}