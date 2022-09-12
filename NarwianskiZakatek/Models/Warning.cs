namespace NarwianskiZakatek.Models
{
    public class Warning
    {
        public int WarningId { get; set; }
        public string Message { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public AppUser User { get; set; } = new AppUser();
        public bool WasDisplayed { get; set; } = false;
    }
}
