using System.ComponentModel.DataAnnotations;

namespace NarwianskiZakatek.Models
{
    public class Post
    {
        public int PostId { get; set; }

        [Display(Name = "Tytuł")]
        public string Title { get; set; }

        [Display(Name = "Data dodania")]
        public DateTime DateCreated { get; set; }

        [Display(Name = "Treść")]
        public string Content { get; set; }

        [Display(Name = "Zdjęcie")]
        public string? PhotoUrl { get; set; }

        public string getDateWithFormat()
        {
            return DateCreated.ToString("dd/MM/yyyy");
        }

        public string getContentPreview()
        {
            return Content.Substring(0, Math.Min(310, Content.Length)) + "...";
        }

        public string getFullPhotoPath()
        {
            return "/graphics/posts/" + PhotoUrl;
        }
    }
}
