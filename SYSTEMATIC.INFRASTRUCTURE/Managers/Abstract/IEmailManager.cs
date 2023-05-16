using SYSTEMATIC.INFRASTRUCTURE.DTOs;

namespace SYSTEMATIC.INFRASTRUCTURE.Managers.Abstract
{
    public interface IEmailManager
    {
        Task<bool> SendRegisterEmail(EmailDataDto data, string verifyURL);
        string GetBasicData(string content);
    }
}
