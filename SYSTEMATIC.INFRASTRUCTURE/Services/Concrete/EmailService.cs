using Microsoft.Extensions.Options;
using SendGrid.Helpers.Mail;
using SendGrid;
using SYSTEMATIC.INFRASTRUCTURE.Services.Abstract;
using SYSTEMATIC.INFRASTRUCTURE.DTOs;

namespace SYSTEMATIC.INFRASTRUCTURE.Services.Concrete
{
    public class EmailService : IEmailService
    {
        private readonly SendGridSettings _settings;

        public EmailService(IOptions<SendGridSettings> settings)
        {
            _settings = settings.Value;
        }

        public async Task SendEmailAsync(EmailDataDto data)
        {
            var client = new SendGridClient(_settings.ApiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress(_settings.FromEmail, _settings.FromName),
                Subject = data.Subject,
                HtmlContent = data.Content
            };
            msg.AddTo(new EmailAddress(data.ToEmail));
            var response = await client.SendEmailAsync(msg);
        }
    }
}
