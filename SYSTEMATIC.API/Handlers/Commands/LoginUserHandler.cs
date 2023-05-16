﻿using SYSTEMATIC.INFRASTRUCTURE.Requests;
using SYSTEMATIC.INFRASTRUCTURE.Responses;
using SYSTEMATIC.INFRASTRUCTURE.Services;

namespace SYSTEMATIC.API.Handlers.Commands
{
    public class LoginUserHandler : IRequestHandler<LoginUserRequest, LoginUserResponse>
    {
        private readonly IAccountService _accountService;

        public LoginUserHandler(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public async Task<LoginUserResponse> Handle(LoginUserRequest request)
        {
            var loginResponse = await _accountService.LoginUserAsync(request);

            return new LoginUserResponse { Token = loginResponse.Token, RefreshToken = loginResponse.RefreshToken};
        }
    }
}
