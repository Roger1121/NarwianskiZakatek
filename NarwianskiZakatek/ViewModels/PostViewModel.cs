using NarwianskiZakatek.CustomAnnotations;
using System.ComponentModel.DataAnnotations;

namespace NarwianskiZakatek.ViewModels
{
    public class PostViewModel
    {
        public int PostId { get; set; }

        [Display(Name = "Tytuł")]
        [RequiredNotNullOrWhitespace(ErrorMessage = "Nie podano tytułu posta")]
        public string Title { get; set; } = string.Empty;

        [Display(Name = "Data dodania")]
        public DateTime DateCreated { get; set; }

        [Display(Name = "Treść")]
        [RequiredNotNullOrWhitespace(ErrorMessage = "Post nie posiada treści")]
        public string Content { get; set; } = string.Empty;

        public string PhotoUrl { get; set; } = string.Empty;

        [Display(Name = "Zdjęcie")]
        public IFormFile? File { get; set; }
    }
}
