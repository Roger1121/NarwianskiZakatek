namespace NarwianskiZakatek.Services
{
    public class EmailConfig
    {
        public string SmtpServer { get; set; } = string.Empty;
        public int SmtpPort { get; set; }
        public bool EnableSsl { get; set; }
        public bool UseDefaultCredentials { get; set; } 
        public string ServerEmail { get; set; } = string.Empty;
        public string ServerPassword { get; set; } = string.Empty;

    }
}
