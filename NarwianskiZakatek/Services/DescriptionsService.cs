using NarwianskiZakatek.Data;
using NarwianskiZakatek.Models;
using NarwianskiZakatek.Services;
using NarwianskiZakatek.ViewModels;

namespace NarwianskiZakatek.Repositories
{
    public class DescriptionsService : IDescriptionsService
    {
        private readonly ApplicationDbContext _context;

        public DescriptionsService(ApplicationDbContext context)
        {
            _context = context;
        }

        public Description GetByTitle(string title)
        {
            return _context.Descriptions.Where(d => d.Title == title).First();
        }

        public void Update(DescriptionViewModel viewModel)
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
                    if (description.PhotoUrl != null)
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
        }
    }
}
