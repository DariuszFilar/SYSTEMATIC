using SYSTEMATIC.INFRASTRUCTURE.DTOs;
using SYSTEMATIC.INFRASTRUCTURE.Managers.Abstract;
using SYSTEMATIC.INFRASTRUCTURE.Services.Abstract;

namespace SYSTEMATIC.INFRASTRUCTURE.Managers.Concrete
{
    public class MailManager : IMailManager
    {
        private readonly IMailService _mailService;
        public MailManager(IMailService mailService)
        {
            _mailService = mailService;
        }

        public async Task<bool> SendRegisterMail(EmailDataDto data, string verificationCode)
        {
            string resourcePath = @"../SYSTEMATIC.INFRASTRUCTURE/Resources/MailTemplates/RegisterMailTemplate.html";
            string template = File.ReadAllText(resourcePath);

            data.Content = template;
            data.Content = data.Content.Replace("#VERIFYURL#", verificationCode);
            data.Subject = "Systematyczny - link aktywacyjny";
            data.Content = GetBasicData(data.Content);

            await _mailService.SendEmailAsync(data);
            return true;
        }

        public string GetBasicData(string content)
        {
            string headerPath = @"../SYSTEMATIC.INFRASTRUCTURE/Resources/MailTemplates/Partial/_header.html";
            string header = File.ReadAllText(headerPath);
            string footerPath = @"../SYSTEMATIC.INFRASTRUCTURE/Resources/MailTemplates/Partial/_footer.html";
            string footer = File.ReadAllText(footerPath);

            content = content.Replace("#HEADER#", header);
            content = content.Replace("#FOOTER#", footer);
            return content;
        }
    }
}
