namespace NarwianskiZakatek.Models
{
    public class Description
    {
        public int DescriptionId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string? PhotoUrl { get; set; }

        public string getFullPhotoPath()
        {
            if (PhotoUrl == null)
                return null;
            return "/graphics/descriptions/" + PhotoUrl;
        }
    }
}
