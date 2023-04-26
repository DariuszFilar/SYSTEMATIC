using SYSTEMATIC.INFRASTRUCTURE.DTOs;

namespace SYSTEMATIC.INFRASTRUCTURE.Services.Abstract
{
    public interface IEmailService
    {
        Task SendEmailAsync(EmailDataDto data);
    }
}
