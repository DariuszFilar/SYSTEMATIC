using SYSTEMATIC.INFRASTRUCTURE.Requests;

namespace SYSTEMATIC.INFRASTRUCTURE.Services
{
    public interface IAccountService
    {
        Task RegisterUserAsync(RegisterUserRequest request);
    }
}
