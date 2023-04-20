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
        private readonly AppSettings _appSettings;
        public AccountService(IUserRepository userRepository,
            IPasswordHasher<User> passwordHasher,
            AppSettings appSettings)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _appSettings = appSettings;
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

            var emailVerificationCode = GenerateEmailVerificationCode();
            newUser.EmailVerificationCode = emailVerificationCode;

            var emailVerificationCodeExpireAt = DateTime.UtcNow.AddDays(_appSettings.EmailVerificationCodeExpirationDays);
            newUser.EmailVerificationCodeExpireAt = emailVerificationCodeExpireAt;

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

        private static string GenerateEmailVerificationCode()
        {
            var emailVerificationCode = Guid.NewGuid().ToString();
            return emailVerificationCode;
        }
    }
}