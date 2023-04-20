using SYSTEMATIC.API.Handlers.Commands;
using SYSTEMATIC.INFRASTRUCTURE.Responses;

namespace SYSTEMATIC.INFRASTRUCTURE.Requests
{
    public class VerifyEmailCodeRequest : IRequest<VerifyEmailCodeResponse>
    {
        public string EmailVerificationCode { get; set; }
    }
}

