using SYSTEMATIC.DB.Entities;

namespace SYSTEMATIC.INFRASTRUCTURE.Repositories.Abstract
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetByEmailAsync(string email);
    }

}
