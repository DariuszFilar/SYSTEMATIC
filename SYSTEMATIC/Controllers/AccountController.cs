using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using SYSTEMATIC.API.Handlers.Commands;
using SYSTEMATIC.INFRASTRUCTURE.Requests;
using SYSTEMATIC.INFRASTRUCTURE.Responses;

namespace SYSTEMATIC.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IRequestHandler<RegisterUserRequest, RegisterUserResponse> _registerRequestHandler;

        public AccountController(IRequestHandler<RegisterUserRequest, RegisterUserResponse> registerRequestHandler)
        {
            _registerRequestHandler = registerRequestHandler;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUserRequest request)
        {
            var response = await _registerRequestHandler.Handle(request);
            return Ok(response);
        }
    }
}
