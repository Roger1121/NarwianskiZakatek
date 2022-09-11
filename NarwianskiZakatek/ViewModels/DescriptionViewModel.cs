using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace NarwianskiZakatek.ViewModels
{
    public class DescriptionViewModel
    {
        [Display(Name = "Tytuł")]
        public string Title { get; set; }

        [Display(Name = "Treść")]
        public string Content { get; set; }

        public IFormFile? File { get; set; }
    }
}
