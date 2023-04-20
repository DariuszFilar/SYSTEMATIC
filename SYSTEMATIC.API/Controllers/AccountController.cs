using Microsoft.AspNetCore.Mvc;
using SYSTEMATIC.API.Handlers.Commands;
using SYSTEMATIC.INFRASTRUCTURE.Requests;
using SYSTEMATIC.INFRASTRUCTURE.Responses;

namespace SYSTEMATIC.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IRequestHandler<RegisterUserRequest, RegisterUserResponse> _registerHandler;
        private readonly IRequestHandler<VerifyEmailCodeRequest, VerifyEmailCodeResponse> _verifyEmailCodeHandler;

        public AccountController(IRequestHandler<RegisterUserRequest, 
            RegisterUserResponse> registerHandler,
            IRequestHandler<VerifyEmailCodeRequest, VerifyEmailCodeResponse> verifyEmailCodeHandler)
        {
            _registerHandler = registerHandler;
            _verifyEmailCodeHandler = verifyEmailCodeHandler;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUserRequest request)
        {
            var response = await _registerHandler.Handle(request);
            return Ok(response);
        }

        [HttpPost("verify-email/{verificationCode}")]
        public async Task<IActionResult> VerifyEmail([FromRoute]string verificationCode)
        {
            var request = new VerifyEmailCodeRequest 
            { 
                EmailVerificationCode = verificationCode 
            };

            var response = await _verifyEmailCodeHandler.Handle(request);
            return Ok(response);
        }
    }
}
