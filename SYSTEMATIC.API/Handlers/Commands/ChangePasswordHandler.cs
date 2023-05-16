using SYSTEMATIC.INFRASTRUCTURE.Requests;
using SYSTEMATIC.INFRASTRUCTURE.Responses;
using SYSTEMATIC.INFRASTRUCTURE.Services.Abstract;

namespace SYSTEMATIC.API.Handlers.Commands
{
    public class ChangePasswordHandler : IRequestWithUserIdHandler<ChangePasswordRequest, ChangePasswordResponse>
    {
        private readonly IAccountService _accountService;
        public ChangePasswordHandler(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public async Task<ChangePasswordResponse> Handle(ChangePasswordRequest request, long userId)
        {
            _ = await _accountService.ChangePasswordAsync(request, userId);

            return new ChangePasswordResponse();
        }
    }
}
