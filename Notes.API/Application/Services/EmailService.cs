using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Notes.API.Application.Services
{
    public class EmailService : IEmailService
    {

        private readonly IOptions<NoteSettings> _settings;

        public EmailService(IOptions<NoteSettings> settings)
        {
            _settings = settings;
        }

        public async Task SendEmailAsync(string to, string subject, string message)
        {
            // create message
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(MailboxAddress.Parse(_settings.Value.SmtpUser));
            emailMessage.To.Add(MailboxAddress.Parse(to));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(TextFormat.Html) { Text = message };

            // send email
            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_settings.Value.SmtpHost, _settings.Value.SmtpPort, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_settings.Value.SmtpUser, _settings.Value.SmtpPass);
            await smtp.SendAsync(emailMessage);
            await smtp.DisconnectAsync(true);
        }
    }
}
