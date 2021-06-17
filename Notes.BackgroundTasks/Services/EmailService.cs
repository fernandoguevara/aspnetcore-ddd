using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Notes.BackgroundTasks.Services
{
    public class EmailService : IEmailService
    {

        private readonly BackgroundTaskSettings _settings;

        public EmailService(IOptions<BackgroundTaskSettings> settings)
        {
            _settings = settings?.Value ?? throw new ArgumentNullException(nameof(settings));
        }

        public async Task SendEmailAsync(string to, string subject, string message, string fileName)
        {
            using var fileStream = File.OpenRead(fileName);
            // create message
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(MailboxAddress.Parse(_settings.SmtpUser));
            emailMessage.To.Add(MailboxAddress.Parse(to));
            emailMessage.Subject = subject;

            var body = new TextPart("plain")
            {
                Text = message
            };

            var attachment = new MimePart("application", "x-xls")
            {
                Content = new MimeContent(fileStream),
                ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                ContentTransferEncoding = ContentEncoding.Base64,
                FileName = Path.GetFileName(fileName)
            };


            var multipart = new Multipart("mixed");
            multipart.Add(body);
            multipart.Add(attachment);

            emailMessage.Body = multipart;

            //maybe i can use dependency injection to optiize this object instanciation...
            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_settings.SmtpHost, _settings.SmtpPort, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_settings.SmtpUser, _settings.SmtpPass);
            await smtp.SendAsync(emailMessage);
            await smtp.DisconnectAsync(true);
        }
    }
}
