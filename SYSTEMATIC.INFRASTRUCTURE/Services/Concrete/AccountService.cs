using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using SYSTEMATIC.API;
using SYSTEMATIC.DB.Entities;
using SYSTEMATIC.INFRASTRUCTURE.DTOs;
using SYSTEMATIC.INFRASTRUCTURE.Exceptions;
using SYSTEMATIC.INFRASTRUCTURE.Managers.Abstract;
using SYSTEMATIC.INFRASTRUCTURE.Repositories.Abstract;
using SYSTEMATIC.INFRASTRUCTURE.Requests;
using SYSTEMATIC.INFRASTRUCTURE.Responses;

namespace SYSTEMATIC.INFRASTRUCTURE.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly AppSettings _appSettings;
        private readonly AuthenticationSettings _authenticationSettings;
        private readonly IMailManager _mailManager;

        public AccountService(IUserRepository userRepository,
            IPasswordHasher<User> passwordHasher,
            AppSettings appSettings,
            AuthenticationSettings authenticationSettings,
            IMailManager mailManager)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _appSettings = appSettings;
            _authenticationSettings = authenticationSettings;
            _mailManager = mailManager;            
        }

        public async Task RegisterUserAsync(RegisterUserRequest request)
        {                       
            User user = await _userRepository.GetByEmailAsync(request.Email);
            if (user != null)
            {
                throw new BadRequestException("Invalid email or password.");
            }

            var newUser = new User()
            {
                Email = request.Email,
                EmialVerificationCodeSendedAt = DateTime.UtcNow
            };

            var hashedPassword = _passwordHasher.HashPassword(newUser, request.Password);
            newUser.PasswordHash = hashedPassword;

            var emailVerificationCode = GenerateEmailVerificationCode();
            newUser.EmailVerificationCode = emailVerificationCode;
            EmailDataDto data = new()
            {
                Content = emailVerificationCode,
                ToEmail = request.Email
            };
            await _mailManager.SendRegisterMail(data, emailVerificationCode);

            var emailVerificationCodeExpireAt = DateTime.UtcNow.AddDays(_appSettings.EmailVerificationCodeExpirationDays);
            newUser.EmailVerificationCodeExpireAt = emailVerificationCodeExpireAt;

            await _userRepository.AddAsync(newUser);
        }

        public async Task<bool> VerifyEmailCodeAsync(VerifyEmailCodeRequest request)
        {
            var user = await _userRepository.GetByEmailVerificationCodeAsync(request.EmailVerificationCode) ?? throw new NotFoundException("User not found");

            if (user.EmailVerificationCodeExpireAt.Value <= DateTime.UtcNow)
            {
                throw new ExpiredException("Email verification code is expired");
            }

            user.EmailVerificationCode = null;
            user.EmailVerificationCodeExpireAt = null;

            await _userRepository.UpdateAsync(user);

            return true;
        }

        private static string GenerateEmailVerificationCode()
        {
            var emailVerificationCode = Guid.NewGuid().ToString();
            return emailVerificationCode;
        }

        public async Task<LoginUserResponse> LoginUserAsync(LoginUserRequest request)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email) ?? throw new BadRequestException("Invalid email or password.");

            PasswordVerificationResult passwordVerificationResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);

            if (passwordVerificationResult == PasswordVerificationResult.Failed)
            {
                throw new BadRequestException("Invalid email or password.");
            }

            if (user.EmailVerificationCode != null)
            {
                throw new BadRequestException("Invalid email or password.");
            }

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSettings.JwtKey));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(_authenticationSettings.JwtExpireDays);

            var token = new JwtSecurityToken(_authenticationSettings.JwtIssuer,
                _authenticationSettings.JwtIssuer,
                claims,
                expires: expires,
                signingCredentials: cred);

            var tokenHandler = new JwtSecurityTokenHandler().WriteToken(token).ToString();

            return new LoginUserResponse { Token = tokenHandler };
        }
    }
}