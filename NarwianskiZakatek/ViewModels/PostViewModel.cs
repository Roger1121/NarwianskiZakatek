using System.ComponentModel.DataAnnotations;

namespace NarwianskiZakatek.ViewModels
{
    public class PostViewModel
    {
        [Display(Name = "Tytuł")]
        public string Title { get; set; }

        [Display(Name = "Data dodania")]
        public DateTime DateCreated { get; set; }

        [Display(Name = "Treść")]
        public string Content { get; set; }

        public IFormFile? File { get; set; }
    }
}
