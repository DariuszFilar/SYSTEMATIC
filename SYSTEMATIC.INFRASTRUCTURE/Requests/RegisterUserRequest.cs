using SYSTEMATIC.API.Handlers.Commands;
using SYSTEMATIC.INFRASTRUCTURE.Responses;

namespace SYSTEMATIC.INFRASTRUCTURE.Requests
{
    public class RegisterUserRequest : IRequest<RegisterUserResponse>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
