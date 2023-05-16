using SYSTEMATIC.DB.Entities;

namespace SYSTEMATIC.INFRASTRUCTURE.Repositories.Abstract
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetByEmailAsync(string email);
        Task<User> GetByEmailVerificationCodeAsync(string verificationCode);
        Task<User> GetByIdAsync(long userId);
        Task<User> GetByRefreshTokenAsync(string refreshToken);
    }

}
