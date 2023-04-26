using SYSTEMATIC.INFRASTRUCTURE.DTOs;

namespace SYSTEMATIC.INFRASTRUCTURE.Services.Abstract
{
    public interface IMailService
    {
        Task SendEmailAsync(EmailMessageDto message);
    }
}
