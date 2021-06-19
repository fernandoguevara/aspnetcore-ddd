
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Notes.Common.Email
{
    public class EmailService : IEmailService
    {

        private readonly IConfiguration _settings;

        public EmailService(IConfiguration settings)
        {
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }

        public async Task SendEmailAsync(string to, string subject, string message, string fileName)
        {
            var emailMessage = BuildEmailMessage(_settings["SmtpUser"], to, message, fileName);
            await SendMessageAsync(emailMessage);
        }

        public async Task SendEmailAsync(string to, string subject, string messagee)
        {
            var emailMessage = BuildEmailMessage(_settings["SmtpUser"], to, messagee);
            await SendMessageAsync(emailMessage);
        }

        private MimeMessage BuildEmailMessage(string from, string to, string message, string fileName = "")
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(MailboxAddress.Parse(_settings["SmtpUser"]));
            emailMessage.To.Add(MailboxAddress.Parse(to));
            var body = BuildBodyMessage(message, fileName);
            emailMessage.Body = body;
            return emailMessage;
        }

        private MimeEntity BuildBodyMessage(string message, string fileName = "")
        {
            var body = new TextPart("plain")
            {
                Text = message
            };

            if (String.IsNullOrWhiteSpace(fileName)) return body;

            using var fileStream = File.OpenRead(fileName);
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

            return multipart;
        }

        private async Task SendMessageAsync(MimeMessage emailMessage)
        {
            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_settings["SmtpHost"], Convert.ToInt32(_settings["SmtpPort"]), SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_settings["SmtpUser"], _settings["SmtpPass"]);
            await smtp.SendAsync(emailMessage);
            await smtp.DisconnectAsync(true);
        }
    }
}
