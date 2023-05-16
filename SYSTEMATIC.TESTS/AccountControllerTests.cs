using FluentValidation.TestHelper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Moq;
using SYSTEMATIC.DB.Entities;
using SYSTEMATIC.INFRASTRUCTURE;
using SYSTEMATIC.INFRASTRUCTURE.Managers.Abstract;
using SYSTEMATIC.INFRASTRUCTURE.Repositories.Abstract;
using SYSTEMATIC.INFRASTRUCTURE.Requests;
using SYSTEMATIC.INFRASTRUCTURE.Services.Concrete;
using SYSTEMATIC.INFRASTRUCTURE.Validators;

namespace SYSTEMATIC.TESTS
{
    public class Tests
    {
        private Mock<IUserRepository> mockUserRepository;
        private Mock<IPasswordHasher<User>> mockPasswordHasher;
        private Mock<IOptions<AppSettings>> mockAppSettings;
        private AccountService accountService;
        private RegisterUserRequestValidator _registerUserRequestValidator;
        private Mock<AuthenticationSettings> mockAuthenticationSettings;
        private Mock<IEmailManager> mockEmailManager;

        [SetUp]
        public void Setup()
        {
            mockUserRepository = new Mock<IUserRepository>();
            mockPasswordHasher = new Mock<IPasswordHasher<User>>();
            mockAppSettings = new Mock<IOptions<AppSettings>>();
            mockAuthenticationSettings = new Mock<AuthenticationSettings>();
            _registerUserRequestValidator = new RegisterUserRequestValidator();
            mockEmailManager = new Mock<IEmailManager>();

            _ = mockAppSettings.Setup(x => x.Value).Returns(new AppSettings
            {
                EmailVerificationCodeExpirationDays = 7
            });

            accountService = new AccountService(mockUserRepository.Object,
                mockPasswordHasher.Object,
                mockAppSettings.Object.Value,
                mockAuthenticationSettings.Object,
                mockEmailManager.Object);
        }

        private static RegisterUserRequest CreateRequestWithCorrectEmail(string password)
        {
            return new RegisterUserRequest
            {
                Email = "test@test.com",
                Password = password
            };
        }

        private static RegisterUserRequest CreateRequestWithCorrectPassword(string email)
        {
            return new RegisterUserRequest
            {
                Email = email,
                Password = "Test123."
            };
        }

        [Test]
        public async Task RegisterUserAsync_ValidRequest_ShouldGenerateEmailVerificationCode()
        {
            // Arrange
            RegisterUserRequest request = new()
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
            RegisterUserRequest request = new()
            {
                Email = "test@test.com",
                Password = "Test123!"
            };

            // Act
            await accountService.RegisterUserAsync(request);

            // Assert
            mockUserRepository.Verify(x => x.AddAsync(It.Is<User>(u => u.EmailVerificationCodeExpireAt.Value.Date == DateTime.UtcNow.AddDays(7).Date)), Times.Once);
        }

        [Test]
        public void Password_Valid_ShouldNotHaveValidationError()
        {
            // Arrange
            RegisterUserRequest request = CreateRequestWithCorrectPassword("test@test.com");

            // Act
            TestValidationResult<RegisterUserRequest> result = _registerUserRequestValidator.TestValidate(request);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Password);
        }

        [Test]
        public void Password_TooShort_ShouldHaveValidationError()
        {
            // Arrange 
            RegisterUserRequest request = CreateRequestWithCorrectEmail("q1.A");

            // Act
            TestValidationResult<RegisterUserRequest> result = _registerUserRequestValidator.TestValidate(request);

            // Assert
            _ = result.ShouldHaveValidationErrorFor(x => x.Password);
        }

        [Test]
        public void Password_NotContainsAtLeastOneDigit_ShouldHaveValidationError()
        {
            // Arrange 
            RegisterUserRequest request = CreateRequestWithCorrectEmail("Test.!");

            // Act
            TestValidationResult<RegisterUserRequest> result = _registerUserRequestValidator.TestValidate(request);

            // Assert
            _ = result.ShouldHaveValidationErrorFor(x => x.Password);
        }

        [Test]
        public void Password_NotContainsAtLeastOneUpperCase_ShouldHaveValidationError()
        {
            // Arrange 
            RegisterUserRequest request = CreateRequestWithCorrectEmail("test1.");

            // Act
            TestValidationResult<RegisterUserRequest> result = _registerUserRequestValidator.TestValidate(request);

            // Assert
            _ = result.ShouldHaveValidationErrorFor(x => x.Password);
        }

        [Test]
        public void Password_NotContainsAtLeastOneLowerCase_ShouldHaveValidationError()
        {
            // Arrange 
            RegisterUserRequest request = CreateRequestWithCorrectEmail("TEST1.");

            // Act
            TestValidationResult<RegisterUserRequest> result = _registerUserRequestValidator.TestValidate(request);

            // Assert
            _ = result.ShouldHaveValidationErrorFor(x => x.Password);
        }

        [Test]
        public void Password_NotContainsAtLeastOneSpecialCharacter_ShouldHaveValidationError()
        {
            // Arrange 
            RegisterUserRequest request = CreateRequestWithCorrectEmail("Test12");

            // Act
            TestValidationResult<RegisterUserRequest> result = _registerUserRequestValidator.TestValidate(request);

            // Assert
            _ = result.ShouldHaveValidationErrorFor(x => x.Password);
        }

        [Test]
        public void Email_Valid_ShouldNotHaveValidationError()
        {
            // Arrange
            RegisterUserRequest request = CreateRequestWithCorrectEmail("Test123.");

            // Act
            TestValidationResult<RegisterUserRequest> result = _registerUserRequestValidator.TestValidate(request);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Email);
        }

        [Test]
        public void Email_NotContainsAt_ShouldHaveValidationError()
        {
            // Arrange 
            RegisterUserRequest request = CreateRequestWithCorrectPassword("Testwp.pl");

            // Act
            TestValidationResult<RegisterUserRequest> result = _registerUserRequestValidator.TestValidate(request);

            // Assert
            _ = result.ShouldHaveValidationErrorFor(x => x.Email);
        }
    }
}