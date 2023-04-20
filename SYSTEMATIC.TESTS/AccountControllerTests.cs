using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Moq;
using SYSTEMATIC.DB.Entities;
using SYSTEMATIC.INFRASTRUCTURE.Repositories.Abstract;
using SYSTEMATIC.INFRASTRUCTURE;
using SYSTEMATIC.INFRASTRUCTURE.Services;
using SYSTEMATIC.INFRASTRUCTURE.Requests;

namespace SYSTEMATIC.TESTS
{
    public class Tests
    {
        private Mock<IUserRepository> mockUserRepository;
        private Mock<IPasswordHasher<User>> mockPasswordHasher;
        private Mock<IOptions<AppSettings>> mockAppSettings;
        private AccountService accountService;

        [SetUp]
        public void Setup()
        {
            mockUserRepository = new Mock<IUserRepository>();
            mockPasswordHasher = new Mock<IPasswordHasher<User>>();
            mockAppSettings = new Mock<IOptions<AppSettings>>();

            mockAppSettings.Setup(x => x.Value).Returns(new AppSettings
            {
                EmailVerificationCodeExpirationDays = 7
            });

            accountService = new AccountService(mockUserRepository.Object, mockPasswordHasher.Object, mockAppSettings.Object.Value);
        }

        [Test]
        public async Task RegisterUserAsync_ValidRequest_ShouldGenerateEmailVerificationCode()
        {
            // Arrange
            var request = new RegisterUserRequest
            {
                Email = "test@test.com",
                Password = "Test123!"
            };

            // Act
            await accountService.RegisterUserAsync(request);

            // Assert
            mockUserRepository.Verify(x => x.AddAsync(It.Is<User>(u => !string.IsNullOrEmpty(u.EmailVerificationCode))), Times.Once);
        }

        [Test]
        public async Task RegisterUserAsync_ValidRequest_ShouldSetEmailVerificationCodeExpireAtTo7Days()
        {
            // Arrange
            var request = new RegisterUserRequest
            {
                Email = "test@test.com",
                Password = "Test123!"
            };

            // Act
            await accountService.RegisterUserAsync(request);

            // Assert
            mockUserRepository.Verify(x => x.AddAsync(It.Is<User>(u => u.EmailVerificationCodeExpireAt.Date == DateTime.UtcNow.AddDays(7).Date)), Times.Once);
        }
    }
}