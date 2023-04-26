using Microsoft.Extensions.Options;
using SendGrid.Helpers.Mail;
using SendGrid;
using SYSTEMATIC.INFRASTRUCTURE.Services.Abstract;
using SYSTEMATIC.INFRASTRUCTURE.DTOs;

namespace SYSTEMATIC.INFRASTRUCTURE.Services.Concrete
{
    public class MailService : IMailService
    {
        private readonly SendGridSettings _settings;

        public MailService(IOptions<SendGridSettings> settings)
        {
            _settings = settings.Value;
        }

        public async Task SendEmailAsync(EmailMessageDto message)
        {
            var client = new SendGridClient(_settings.ApiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress(_settings.FromEmail, _settings.FromName),
                Subject = message.Subject,
                HtmlContent = message.Content
            };
            msg.AddTo(new EmailAddress(message.ToEmail));
            var response = await client.SendEmailAsync(msg);
        }
    }
}
