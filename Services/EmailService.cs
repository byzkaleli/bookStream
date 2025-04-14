using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;
using bookStream.Configurations;

namespace bookStream.Services
{
    public class EmailService
    {
        private readonly EmailSettings _emailSettings;

        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public void SendVerificationEmail(string email, string verificationLink)
        {
            using var smtpClient = new SmtpClient(_emailSettings.SmtpServer)
            {
                Port = _emailSettings.Port,
                Credentials = new NetworkCredential(_emailSettings.Username, _emailSettings.Password),
                EnableSsl = _emailSettings.UseSsl
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_emailSettings.FromAddress),
                Subject = "Verify Your Email",
                Body = $"<h1>Email Verification</h1><p>Click the link below to verify your email:</p><a href='{verificationLink}'>Verify Email</a>",
                IsBodyHtml = true
            };

            mailMessage.To.Add(email);
            smtpClient.Send(mailMessage);
        }
    }
}