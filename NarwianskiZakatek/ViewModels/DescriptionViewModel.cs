using NarwianskiZakatek.CustomAnnotations;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace NarwianskiZakatek.ViewModels
{
    public class DescriptionViewModel
    {
        [Display(Name = "Tytuł")]
        public string Title { get; set; } = string.Empty;

        [Display(Name = "Treść")]
        [RequiredNotNullOrWhitespace(ErrorMessage = "Wpis nie posiada treści")]
        public string Content { get; set; } = string.Empty;

        [Display(Name = "Zdjęcie")]
        public IFormFile? File { get; set; }
    }
}
