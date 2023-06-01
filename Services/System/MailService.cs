using AutoServiceMVC.Models.System;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;

namespace AutoServiceMVC.Services.System
{
    public interface IMailService
    {
        public Task SendMailAsync(MailContent content);
    }

    public class MailService : IMailService
    {
        private readonly MailSettings _mailSettings;
        private readonly ILogger<IMailService> _logger;

        public MailService(IOptions<MailSettings> mailSettings, ILogger<IMailService> logger) 
        {
            _mailSettings = mailSettings.Value;
            _logger = logger;
        }

        public async Task SendMailAsync(MailContent mailContent)
        {
            var email = new MimeMessage();
            email.Sender = new MailboxAddress(_mailSettings.DisplayName, _mailSettings.Mail);
            email.From.Add(new MailboxAddress(_mailSettings.DisplayName, _mailSettings.Mail));
            email.To.Add(MailboxAddress.Parse(mailContent.To));
            email.Subject = mailContent.Subject;


            var builder = new BodyBuilder();
            builder.HtmlBody = mailContent.Body;
            email.Body = builder.ToMessageBody();

            // dùng SmtpClient của MailKit
            using var smtp = new MailKit.Net.Smtp.SmtpClient();

            try
            {
                await smtp.ConnectAsync(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(_mailSettings.Mail, _mailSettings.Password);
                await smtp.SendAsync(email);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Send mail fail");
                _logger.LogError(ex.Message);
            }

            await smtp.DisconnectAsync(true);
        }
    }
}
