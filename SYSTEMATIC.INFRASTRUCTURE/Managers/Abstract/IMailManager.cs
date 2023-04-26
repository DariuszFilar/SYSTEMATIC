using System.Net.NetworkInformation;
using SYSTEMATIC.INFRASTRUCTURE.DTOs;

namespace SYSTEMATIC.INFRASTRUCTURE.Managers.Abstract
{
    public interface IMailManager
    {
        Task<bool> SendRegisterMail(EmailDataDto data, string verifyURL);
        string GetBasicData(string content);
    }
}
