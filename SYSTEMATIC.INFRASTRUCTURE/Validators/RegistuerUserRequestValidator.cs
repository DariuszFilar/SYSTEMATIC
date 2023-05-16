using FluentValidation;
using System.Text.RegularExpressions;
using SYSTEMATIC.INFRASTRUCTURE.Requests;

namespace SYSTEMATIC.INFRASTRUCTURE.Validators
{
    public class RegisterUserRequestValidator : AbstractValidator<RegisterUserRequest>
    {
        public RegisterUserRequestValidator()
        {
            _ = RuleFor(x => x.Email).NotEmpty().EmailAddress();
            _ = RuleFor(x => x.Password)
                .Must(password => Regex.IsMatch(password, "^(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*\\W).{6,50}$"))
                .WithMessage("Your password must be between 6 and 50 characters, and include at least one digit, one lowercase letter, one uppercase letter, and one special character.");
        }
    }
}