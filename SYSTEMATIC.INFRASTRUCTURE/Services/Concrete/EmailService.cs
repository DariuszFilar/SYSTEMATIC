using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using SYSTEMATIC.INFRASTRUCTURE.DTOs;
using SYSTEMATIC.INFRASTRUCTURE.Services.Abstract;

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
            SendGridClient client = new(_settings.ApiKey);
            SendGridMessage msg = new()
            {
                From = new EmailAddress(_settings.FromEmail, _settings.FromName),
                Subject = data.Subject,
                HtmlContent = data.Content
            };
            msg.AddTo(new EmailAddress(data.ToEmail));
            _ = await client.SendEmailAsync(msg);
        }
    }
}
