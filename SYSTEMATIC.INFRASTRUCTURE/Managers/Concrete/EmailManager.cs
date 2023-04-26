using SYSTEMATIC.INFRASTRUCTURE.DTOs;
using SYSTEMATIC.INFRASTRUCTURE.Managers.Abstract;
using SYSTEMATIC.INFRASTRUCTURE.Services.Abstract;

namespace SYSTEMATIC.INFRASTRUCTURE.Managers.Concrete
{
    public class EmailManager : IEmailManager
    {
        private readonly IEmailService _emailService;
        public EmailManager(IEmailService emailService)
        {
            _emailService = emailService;
        }

        public async Task<bool> SendRegisterEmail(EmailDataDto data, string verificationCode)
        {
            string resourcePath = @"../SYSTEMATIC.INFRASTRUCTURE/Resources/EmailTemplates/RegisterEmailTemplate.html";
            string template = File.ReadAllText(resourcePath);

            data.Content = template;
            data.Content = data.Content.Replace("#VERIFYURL#", verificationCode);
            data.Subject = "Systematyczny - link aktywacyjny";
            data.Content = GetBasicData(data.Content);

            await _emailService.SendEmailAsync(data);
            return true;
        }

        public string GetBasicData(string content)
        {
            string headerPath = @"../SYSTEMATIC.INFRASTRUCTURE/Resources/EmailTemplates/Partial/_header.html";
            string header = File.ReadAllText(headerPath);
            string footerPath = @"../SYSTEMATIC.INFRASTRUCTURE/Resources/EmailTemplates/Partial/_footer.html";
            string footer = File.ReadAllText(footerPath);

            content = content.Replace("#HEADER#", header);
            content = content.Replace("#FOOTER#", footer);
            return content;
        }
    }
}
