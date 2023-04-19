using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;
using SYSTEMATIC.DB.Entities;
using SYSTEMATIC.INFRASTRUCTURE.Repositories.Abstract;
using SYSTEMATIC.INFRASTRUCTURE.Requests;

namespace SYSTEMATIC.INFRASTRUCTURE.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        public AccountService(IUserRepository userRepository,
            IPasswordHasher<User> passwordHasher)
        {
            _userRepository = userRepository;
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

            await _userRepository.AddAsync(newUser);
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