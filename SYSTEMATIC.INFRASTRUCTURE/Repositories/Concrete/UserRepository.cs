using Microsoft.EntityFrameworkCore;
using SYSTEMATIC.DB;
using SYSTEMATIC.DB.Entities;
using SYSTEMATIC.INFRASTRUCTURE.Repositories.Abstract;

namespace SYSTEMATIC.INFRASTRUCTURE.Repositories.Concrete
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly SystematicDbContext _context;
        public UserRepository(SystematicDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}
