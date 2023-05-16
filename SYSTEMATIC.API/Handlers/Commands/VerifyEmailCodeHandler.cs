using SYSTEMATIC.INFRASTRUCTURE.Requests;
using SYSTEMATIC.INFRASTRUCTURE.Responses;
using SYSTEMATIC.INFRASTRUCTURE.Services.Abstract;

namespace SYSTEMATIC.API.Handlers.Commands
{
    public class VerifyEmailCodeHandler : IRequestHandler<VerifyEmailCodeRequest, VerifyEmailCodeResponse>
    {
        private readonly IAccountService _accountService;

        public VerifyEmailCodeHandler(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public async Task<VerifyEmailCodeResponse> Handle(VerifyEmailCodeRequest request)
        {
            _ = await _accountService.VerifyEmailCodeAsync(request);
            return new VerifyEmailCodeResponse();
        }
    }
}
