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
        private readonly IRequestHandler<RegisterUserRequest, RegisterUserResponse> _handler;

        public AccountController(IRequestHandler<RegisterUserRequest, RegisterUserResponse> handler)
        {
            _handler = handler;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUserRequest request)
        {
            var response = await _handler.Handle(request);
            return Ok(response);
        }
    }
}
