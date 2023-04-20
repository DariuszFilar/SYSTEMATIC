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
            _context = dbContext;
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User> GetByEmailVerificationCodeAsync(string verificationCode)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.EmailVerificationCode == verificationCode);
        }
    }
}
