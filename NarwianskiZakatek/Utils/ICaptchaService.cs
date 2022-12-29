namespace NarwianskiZakatek.Services
{
    public interface ICaptchaService
    {
        Task<bool> IsValid(string captcha);
    }
}
