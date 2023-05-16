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
        private readonly IEmailManager _emailManager;

        public AccountService(IUserRepository userRepository,
            IPasswordHasher<User> passwordHasher,
            AppSettings appSettings,
            AuthenticationSettings authenticationSettings,
            IEmailManager emailManager)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _appSettings = appSettings;
            _authenticationSettings = authenticationSettings;
            _emailManager = emailManager;            
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
            await _emailManager.SendRegisterEmail(data, emailVerificationCode);

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

        public async Task<LoginUserResponse> LoginUserAsync(LoginUserRequest request)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email) ?? throw new BadRequestException("Invalid email or password.");

            CheckIfPasswordIsCorrect(user, request.Password);

            var token = GenerateJwtToken(user);

            var newRefreshToken = await GenerateAndSaveRefreshTokenAsync(user);  

            return new LoginUserResponse { Token = token, RefreshToken = newRefreshToken };
        }

        public async Task<ChangePasswordResponse> ChangePasswordAsync(ChangePasswordRequest request, long userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);

            CheckIfPasswordIsCorrect(user, request.OldPassword);

            var hashedPassword = _passwordHasher.HashPassword(user, request.NewPassword);
            user.PasswordHash = hashedPassword;
            await _userRepository.UpdateAsync(user);

            return new ChangePasswordResponse();
        }

        public async Task<(string, string)> RefreshTokenAsync(string refreshToken)
        {
            User user = await _userRepository.GetByRefreshTokenAsync(refreshToken);

            if (user == null)
            {
                throw new BadRequestException("Invalid refresh token.");
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSettings.JwtKey)),
                ValidateIssuer = true,
                ValidIssuer = _authenticationSettings.JwtIssuer,
                ValidateAudience = true,
                ValidAudience = _authenticationSettings.JwtIssuer,
                ClockSkew = TimeSpan.Zero
            };

            try
            {
                SecurityToken validatedToken;
                var principal = tokenHandler.ValidateToken(refreshToken, validationParameters, out validatedToken);

                var newAccessToken = GenerateJwtToken(user);
                var newRefreshToken = await GenerateAndSaveRefreshTokenAsync(user);

                return (newAccessToken, newRefreshToken);
            }
            catch (SecurityTokenExpiredException)
            {
                throw new BadRequestException("Token has expired.");
            }
            catch (SecurityTokenInvalidSignatureException)
            {
                throw new BadRequestException("Invalid token signature.");
            }
            catch (SecurityTokenValidationException)
            {
                throw new BadRequestException("Invalid token.");
            }                       
        }

        private void CheckIfPasswordIsCorrect(User user, string password)
        {
            PasswordVerificationResult passwordVerificationResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);

            if (passwordVerificationResult == PasswordVerificationResult.Failed)
            {
                throw new BadRequestException("Invalid email or password.");
            }

            if (user.EmailVerificationCode != null)
            {
                throw new BadRequestException("Invalid email or password.");
            }
        }           
       
        private async Task<string> GenerateAndSaveRefreshTokenAsync(User user)
        {
            var newRefreshToken = Guid.NewGuid().ToString();
            user.RefreshToken = newRefreshToken;
            await _userRepository.UpdateAsync(user);

            return Guid.NewGuid().ToString();
        }

        private static string GenerateEmailVerificationCode()
        {
            var emailVerificationCode = Guid.NewGuid().ToString();
            return emailVerificationCode;
        }

        private string GenerateJwtToken(User user)
        {
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

            return tokenHandler;
        }
    }
}