using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;
using System.Text;
using SYSTEMATIC.DB;
using SYSTEMATIC.DB.Entities;
using SYSTEMATIC.INFRASTRUCTURE.Requests;

namespace SYSTEMATIC.INFRASTRUCTURE.Services
{
    public class AccountService : IAccountService
    {
        private readonly SystematicDbContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;
        public AccountService(SystematicDbContext context,
            IPasswordHasher<User> passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }
        public async Task RegisterUserAsync(RegisterUserRequest request)
        {
            var newUser = new User()
            {
                Email = request.Email
            };

            var salt = GenerateSalt();
            var hashedPassword = HashPassword(request.Password, salt);

            newUser.PasswordHash = hashedPassword;
            newUser.PasswordSalt = salt;

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();
        }

        private static string GenerateSalt()
        {
            var randomBytes = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
            }

            return Convert.ToBase64String(randomBytes);
        }

        private string HashPassword(string password, string salt)
        {
            return _passwordHasher.HashPassword(null, password + salt);
        }
    }
}