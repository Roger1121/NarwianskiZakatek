namespace NarwianskiZakatek.Models
{
    public class Post
    {
        public int PostId { get; set; }

        public string Title { get; set; } = string.Empty;

        public DateTime DateCreated { get; set; }

        public string Content { get; set; } = string.Empty;

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
            if (PhotoUrl == null)
                return null;
            return "/graphics/posts/" + PhotoUrl;
        }
    }
}
