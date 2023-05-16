using SYSTEMATIC.INFRASTRUCTURE.Requests;
using SYSTEMATIC.INFRASTRUCTURE.Responses;
using SYSTEMATIC.INFRASTRUCTURE.Services.Abstract;

namespace SYSTEMATIC.API.Handlers.Commands
{
    public class RegisterUserHandler : IRequestHandler<RegisterUserRequest, RegisterUserResponse>
    {
        private readonly IAccountService _accountService;

        public RegisterUserHandler(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public async Task<RegisterUserResponse> Handle(RegisterUserRequest request)
        {
            await _accountService.RegisterUserAsync(request);

            return new RegisterUserResponse();
        }
    }
}
