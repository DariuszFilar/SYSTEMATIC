using SYSTEMATIC.DB.Entities;
using SYSTEMATIC.INFRASTRUCTURE.Requests;
using SYSTEMATIC.INFRASTRUCTURE.Responses;

namespace SYSTEMATIC.INFRASTRUCTURE.Services
{
    public interface IAccountService
    {
        Task RegisterUserAsync(RegisterUserRequest request);
        Task<bool> VerifyEmailCodeAsync(VerifyEmailCodeRequest request);
        Task<LoginUserResponse> LoginUserAsync(LoginUserRequest request);
        Task<ChangePasswordResponse> ChangePasswordAsync(ChangePasswordRequest request, long userId);
    }
}
