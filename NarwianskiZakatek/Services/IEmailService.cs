using NarwianskiZakatek.Models;

namespace NarwianskiZakatek.Services
{
    public interface IEmailService
    {
        Task SendConfirmationEmailAsync(string email, string url);
        Task CancelReservationAsync(string recipient, Reservation reservation);

        Task ConfirmReservationAsync(string recipient, Reservation reservation);

        Task ResetPasswordAsync(string email, string url);

        Task ChangeEmailAddress(string email, string newMail, string url);
    }
}
