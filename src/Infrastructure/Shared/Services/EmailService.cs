using Application.DTOs.Email;
using Application.Interfaces;
using Domain.Settings;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Infrastructure.Shared.Services
{
    public class EmailService : IEmailService
    {
        private readonly MailSettings _mailSettings;

        public EmailService(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }

        public async Task SendAsync(EmailRequest request)
        {
            MailAddress from = new MailAddress(_mailSettings.Email, _mailSettings.DisplayName);
            MailAddress to = new MailAddress(request.To);
            MailMessage mail = new MailMessage(from, to);
            mail.Subject = request.Subject;
            mail.Body = request.Body;

            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.Credentials = new NetworkCredential(_mailSettings.Email, _mailSettings.Password);
            smtp.EnableSsl = true;
            await smtp.SendMailAsync(mail);
        }
    }
}
