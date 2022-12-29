using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using NarwianskiZakatek.Data;
using NarwianskiZakatek.Models;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace NarwianskiZakatek.Services
{
    public class EmailService : IEmailService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly EmailConfig _config;
        public EmailService(UserManager<AppUser> userManager, EmailConfig config, ApplicationDbContext context)
        {
            _userManager = userManager;
            _config = config;
            _context = context;
        }

        public async Task SendConfirmationEmailAsync(string email, string url)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user != null)
            {
                var userId = await _userManager.GetUserIdAsync(user);
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = url + "?userId=" + userId + "&code=" + code + "&returnUrl=%2F";
                SmtpClient client = getSmtpClient();
                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress("narwianskizakatek@gmail.com");
                mailMessage.To.Add(email);
                mailMessage.Body = $"Dziękujemy za rejestrację na naszej stronie.\n Kliknij w poniższy link, aby aktywować konto: \n\n {callbackUrl} \n\n Jeżeli nie zakładałeś(aś) konta na naszej stronie, zignoruj tę wiadomość.\n\nPozdrawiamy\nGospodarstwo agroturystyczne Narwiański Zakątek.";
                mailMessage.Subject = "Link aktywacyjny";
                await client.SendMailAsync(mailMessage);
            }
        }

        public async Task CancelReservationAsync(string recipient, Reservation reservation)
        {
            SmtpClient client = getSmtpClient();
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("narwianskizakatek@gmail.com");
            mailMessage.To.Add(recipient);
            StringBuilder msgBody = new StringBuilder("Rezerwacja została anulowana.\n");
            msgBody.Append("\nId rezerwacji: ");
            msgBody.Append(reservation.ReservationId);
            msgBody.Append("\nData przyjazdu: ");
            msgBody.Append(reservation.BeginDate);
            msgBody.Append("\nData wyjazdu: ");
            msgBody.Append(reservation.EndDate);
            msgBody.Append("\n\nPozdrawiamy");
            msgBody.Append("\nGospodarstwo agroturystyczne Narwiański Zakątek.");
            mailMessage.Body = msgBody.ToString();
            mailMessage.Subject = "Potwierdzenie anulowania rezerwacji";
            await client.SendMailAsync(mailMessage);
        }

        public async Task ConfirmReservationAsync(string recipient, Reservation reservation)
        {
            var reservedRooms = _context.ReservedRooms.Where(r => r.ReservationId == reservation.ReservationId);
            List<int> roomIds = new List<int>();
            foreach (var room in reservedRooms)
            {
                roomIds.Add(room.RoomId);
            }
            var rooms = _context.Rooms.Where(r => roomIds.Contains(r.RoomId));
            SmtpClient client = getSmtpClient();
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("narwianskizakatek@gmail.com");
            mailMessage.To.Add(recipient);
            StringBuilder msgBody = new StringBuilder("Dziękujemy za złożenie rezerwacji.\n");
            msgBody.Append("\nId rezerwacji: ");
            msgBody.Append(reservation.ReservationId);
            msgBody.Append("\nData przyjazdu: ");
            msgBody.Append(reservation.BeginDate);
            msgBody.Append("\nData wyjazdu: ");
            msgBody.Append(reservation.EndDate);
            msgBody.Append("\nZarezerwowane pokoje:\n");
            foreach(var room in rooms)
            {
                msgBody.Append("\n    Numer pokoju: ");
                msgBody.Append(room.RoomNumber);
                msgBody.Append("\n    Liczba łóżek: ");
                msgBody.Append(room.RoomCapacity);
            }
            msgBody.Append("\n\nKwota do zapłaty: ");
            msgBody.Append(reservation.Price);
            msgBody.Append(" zł");
            msgBody.Append("\n\nJeżeli chcesz anulować rezerwację, możesz to zrobić w zakładce 'Rezerwacje' po zalogowaniu na stronie.");
            msgBody.Append("\n\nPozdrawiamy");
            msgBody.Append("\nGospodarstwo agroturystyczne Narwiański Zakątek.");
            mailMessage.Body = msgBody.ToString();
            mailMessage.Subject = "Potwierdzenie złożenia rezerwacji";
            await client.SendMailAsync(mailMessage);
        }

        public async Task ResetPasswordAsync(string email, string url)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user != null)
            {
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = url + "?code=" + code + "&returnUrl=%2F";
                SmtpClient client = getSmtpClient();
                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress("narwianskizakatek@gmail.com");
                mailMessage.To.Add(email);
                mailMessage.Body = $"Kliknij w poniższy link, aby ustawić nowe hasło: \n\n {callbackUrl} \n\n Jeżeli nie prosiłeś o zmianę hasła do konta na naszej stronie, zignoruj tę wiadomość.\n\nPozdrawiamy\nGospodarstwo agroturystyczne Narwiański Zakątek.";
                mailMessage.Subject = "Prośba o zmianę hasła";
                await client.SendMailAsync(mailMessage);
            }
        }

        public async Task ChangeEmailAddress(string email, string newMail, string url)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user != null)
            {
                var userId = await _userManager.GetUserIdAsync(user);
                var code = await _userManager.GenerateChangeEmailTokenAsync(user, newMail);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = url + "?userId=" + userId + "&email=" + newMail + "&code=" + code + "&returnUrl=%2F";
                SmtpClient client = getSmtpClient();
                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress("narwianskizakatek@gmail.com");
                mailMessage.To.Add(newMail);
                mailMessage.Body = $"Kliknij w poniższy link, aby zmienić adres email: \n\n {callbackUrl} \n\n Jeżeli nie prosiłeś o zmianę adresu e-mail do konta na naszej stronie, zignoruj tę wiadomość.\n\nPozdrawiamy\nGospodarstwo agroturystyczne Narwiański Zakątek.";
                mailMessage.Subject = "Prośba o zmianę adresu e-mail";
                await client.SendMailAsync(mailMessage);
            }
        }

        private SmtpClient getSmtpClient()
        {
            SmtpClient client = new SmtpClient(_config.SmtpServer, _config.SmtpPort);
            client.EnableSsl = _config.EnableSsl;
            client.UseDefaultCredentials = _config.UseDefaultCredentials;
            client.Credentials = new NetworkCredential(_config.ServerEmail, _config.ServerPassword);
            return client;
        }
    }
}
