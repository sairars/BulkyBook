using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity.UI.Services;
using MimeKit;
using MailKit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace BulkyBook.Utility
{
    public class EmailSender : IEmailSender
    {
        private readonly MailKitSettings _mailKitSettings;

        public EmailSender(IOptions<MailKitSettings> mailKitSettings)
        {
            _mailKitSettings = mailKitSettings.Value 
                                ?? throw new ArgumentException("Argument is null", 
                                                                nameof(mailKitSettings));
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var emailToSend = new MimeMessage();
            emailToSend.From.Add(MailboxAddress.Parse("hello@bulky.com"));
            emailToSend.To.Add(MailboxAddress.Parse(email));
            emailToSend.Subject = subject;
            emailToSend.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = htmlMessage };

            using SmtpClient emailClient = new();
            emailClient.Connect(_mailKitSettings.SmtpHost, _mailKitSettings.SmtpPort,
                                MailKit.Security.SecureSocketOptions.StartTls);
            emailClient.Authenticate(_mailKitSettings.Username, _mailKitSettings.Password);
            emailClient.Send(emailToSend);
            emailClient.Disconnect(true);

            return Task.CompletedTask;
        }
    }
}
