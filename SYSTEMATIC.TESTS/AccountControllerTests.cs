using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Moq;
using SYSTEMATIC.DB.Entities;
using SYSTEMATIC.INFRASTRUCTURE.Repositories.Abstract;
using SYSTEMATIC.INFRASTRUCTURE;
using SYSTEMATIC.INFRASTRUCTURE.Services;
using SYSTEMATIC.INFRASTRUCTURE.Requests;
using SYSTEMATIC.INFRASTRUCTURE.Validators;
using FluentValidation.TestHelper;
using SYSTEMATIC.API;

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

        [SetUp]
        public void Setup()
        {
            mockUserRepository = new Mock<IUserRepository>();
            mockPasswordHasher = new Mock<IPasswordHasher<User>>();
            mockAppSettings = new Mock<IOptions<AppSettings>>();
            mockAuthenticationSettings = new Mock<AuthenticationSettings>();
            _registerUserRequestValidator = new RegisterUserRequestValidator();

            mockAppSettings.Setup(x => x.Value).Returns(new AppSettings
            {
                EmailVerificationCodeExpirationDays = 7
            });

            accountService = new AccountService(mockUserRepository.Object, mockPasswordHasher.Object, mockAppSettings.Object.Value, mockAuthenticationSettings.Object);
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
            mockUserRepository.Verify(x => x.AddAsync(It.Is<User>(u => u.EmailVerificationCodeExpireAt.Value.Date == DateTime.UtcNow.AddDays(7).Date)), Times.Once);
        }

        [Test]
        public void Password_Valid_ShouldNotHaveValidationError()
        {
            // Arrange
            var request = CreateRequestWithCorrectPassword("test@test.com");

            // Act
            var result = _registerUserRequestValidator.TestValidate(request);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Password);
        }

        [Test]
        public void Password_TooShort_ShouldHaveValidationError()
        {
            // Arrange 
            var request = CreateRequestWithCorrectEmail("q1.A");

            // Act
            var result = _registerUserRequestValidator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Password);
        }

        [Test]
        public void Password_NotContainsAtLeastOneDigit_ShouldHaveValidationError()
        {
            // Arrange 
            var request = CreateRequestWithCorrectEmail("Test.!");

            // Act
            var result = _registerUserRequestValidator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Password);
        }

        [Test]
        public void Password_NotContainsAtLeastOneUpperCase_ShouldHaveValidationError()
        {
            // Arrange 
            var request = CreateRequestWithCorrectEmail("test1.");

            // Act
            var result = _registerUserRequestValidator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Password);
        }

        [Test]
        public void Password_NotContainsAtLeastOneLowerCase_ShouldHaveValidationError()
        {
            // Arrange 
            var request = CreateRequestWithCorrectEmail("TEST1.");

            // Act
            var result = _registerUserRequestValidator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Password);
        }

        [Test]
        public void Password_NotContainsAtLeastOneSpecialCharacter_ShouldHaveValidationError()
        {
            // Arrange 
            var request = CreateRequestWithCorrectEmail("Test12");

            // Act
            var result = _registerUserRequestValidator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Password);
        }

        [Test]
        public void Email_Valid_ShouldNotHaveValidationError()
        {
            // Arrange
            var request = CreateRequestWithCorrectEmail("Test123.");

            // Act
            var result = _registerUserRequestValidator.TestValidate(request);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Email);
        }

        [Test]
        public void Email_NotContainsAt_ShouldHaveValidationError()
        {
            // Arrange 
            var request = CreateRequestWithCorrectPassword("Testwp.pl");

            // Act
            var result = _registerUserRequestValidator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Email);
        }
    }
}