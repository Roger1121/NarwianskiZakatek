using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using NarwianskiZakatek.Migrations;
using NarwianskiZakatek.Models;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace NarwianskiZakatek.Services
{
    public class EmailService : IEmailService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly EmailConfig _config;
        public EmailService(UserManager<AppUser> userManager, EmailConfig config)
        {
            _userManager = userManager;
            _config = config;
        }

        public async Task SendConfirmationEmailAsync(string email, string url)
        {
            var user = _userManager.FindByEmailAsync(email).GetAwaiter().GetResult();

            if (user != null)
            {
                var userId = _userManager.GetUserIdAsync(user).Result;
                var code = _userManager.GenerateEmailConfirmationTokenAsync(user).Result;
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = url + "?userId=" + userId + "&code=" + code + "&returnUrl=%2F";
                SmtpClient client = getSmtpClient();
                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress("narwianskizakatek@gmail.com");
                mailMessage.To.Add(email);
                mailMessage.Body = $"Dziękujemy za rejestrację na naszej stronie.\r\n Kliknij w poniższy link, aby aktywować konto: \r\n\r\n {callbackUrl} \r\n\r\n Jeżeli nie zakładałeś(aś) konta na naszej stronie, zignoruj tę wiadomość.";
                mailMessage.Subject = "Link aktywacyjny";
                client.Send(mailMessage);
            }
        }

        public async Task ConfirmReservation(string recipient, Reservation reservation)
        {
            Task.Run(() =>
            {
                SmtpClient client = getSmtpClient();
                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress("narwianskizakatek@gmail.com");
                mailMessage.To.Add(recipient);
                mailMessage.Body = "Dziękujemy za złożenie rezerwacji: ";  //tresc wiadomosci
                mailMessage.Subject = "Potwierdzenie złożenia rezerwacji";
                client.Send(mailMessage);
            });
        }

        public async Task ResetPassword(string email, string url)
        {
            var user = _userManager.FindByEmailAsync(email).GetAwaiter().GetResult();

            if (user != null)
            {
                var code = _userManager.GeneratePasswordResetTokenAsync(user).Result;
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = url + "?code=" + code + "&returnUrl=%2F";
                SmtpClient client = getSmtpClient();
                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress("narwianskizakatek@gmail.com");
                mailMessage.To.Add(email);
                mailMessage.Body = $"Kliknij w poniższy link, aby ustawić nowe hasło: \r\n\r\n {callbackUrl} \r\n\r\n Jeżeli nie prosiłeś o zmianę hasła do konta na naszej stronie, zignoruj tę wiadomość.";
                mailMessage.Subject = "Prośba o zmianę hasła";
                client.Send(mailMessage);
            }
        }

        public async Task ChangeEmailAddress(string email, string newMail, string url)
        {
            //var userId = await _userManager.GetUserIdAsync(user);
            //var code = await _userManager.GenerateChangeEmailTokenAsync(user, Input.NewEmail);
            //code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            var user = _userManager.FindByEmailAsync(email).GetAwaiter().GetResult();

            if (user != null)
            {
                var userId = _userManager.GetUserIdAsync(user).Result;
                var code = _userManager.GenerateChangeEmailTokenAsync(user, newMail).Result;
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = url + "?userId=" + userId + "&email=" + newMail + "&code=" + code + "&returnUrl=%2F";
                SmtpClient client = getSmtpClient();
                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress("narwianskizakatek@gmail.com");
                mailMessage.To.Add(newMail);
                mailMessage.Body = $"Kliknij w poniższy link, aby zmienić adres email: \r\n\r\n {callbackUrl} \r\n\r\n Jeżeli nie prosiłeś o zmianę adresu e-mail do konta na naszej stronie, zignoruj tę wiadomość.";
                mailMessage.Subject = "Prośba o zmianę adresu e-mail";
                client.Send(mailMessage);
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
