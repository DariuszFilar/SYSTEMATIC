using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
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
        private readonly IRequestHandler<LoginUserRequest, LoginUserResponse> _loginHandler;
        private readonly IRequestWithUserIdHandler<ChangePasswordRequest, ChangePasswordResponse> _changePasswordHandler;

        public AccountController(IRequestHandler<RegisterUserRequest, RegisterUserResponse> registerHandler,
            IRequestHandler<VerifyEmailCodeRequest, VerifyEmailCodeResponse> verifyEmailCodeHandler,
            IRequestHandler<LoginUserRequest, LoginUserResponse> loginHandler,
            IRequestWithUserIdHandler<ChangePasswordRequest, ChangePasswordResponse> changePasswordHandler)
        {
            _registerHandler = registerHandler;
            _verifyEmailCodeHandler = verifyEmailCodeHandler;
            _loginHandler = loginHandler;
            _changePasswordHandler = changePasswordHandler;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUserRequest request)
        {
            RegisterUserResponse response = await _registerHandler.Handle(request);
            return Ok(response);
        }

        [HttpPost("verify-email/{verificationCode}")]
        public async Task<IActionResult> VerifyEmail([FromRoute] string verificationCode)
        {
            VerifyEmailCodeRequest request = new()
            {
                EmailVerificationCode = verificationCode
            };

            VerifyEmailCodeResponse response = await _verifyEmailCodeHandler.Handle(request);
            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginUserRequest request)
        {
            LoginUserResponse response = await _loginHandler.Handle(request);
            return Ok(response);
        }

        [Authorize]
        [HttpPost("changePassword")]
        public async Task<IActionResult> ChangePasswordPassword(ChangePasswordRequest request)
        {
            long userId = long.Parse(User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);
            ChangePasswordResponse response = await _changePasswordHandler.Handle(request, userId);
            return Ok(response);
        }
    }
}
