using NarwianskiZakatek.Models;

namespace NarwianskiZakatek.Services
{
    public interface IEmailService
    {
        Task SendConfirmationEmailAsync(string email, string url);

        Task ConfirmReservation(string recipient, Reservation reservation);

        Task ResetPassword(string email, string url);

        Task ChangeEmailAddress(string email, string newMail, string url);
    }
}
