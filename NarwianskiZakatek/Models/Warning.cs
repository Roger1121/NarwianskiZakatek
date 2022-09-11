namespace NarwianskiZakatek.Models
{
    public class Warning
    {
        public int WarningId { get; set; }
        public string Message { get; set; }
        public string UserId { get; set; }
        public AppUser User { get; set; }
        public bool WasDisplayed { get; set; } = false;
    }
}
