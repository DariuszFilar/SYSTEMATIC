using SYSTEMATIC.API.Handlers.Commands;
using SYSTEMATIC.INFRASTRUCTURE.Responses;

namespace SYSTEMATIC.INFRASTRUCTURE.Requests
{
    public class RegisterUserRequest : IRequest<RegisterUserResponse>
    {
        private string _email;
        private string _password;

        public string Email
        {
            get { return _email; }
            set { _email = value?.Trim(); }
        }

        public string Password
        {
            get { return _password; }
            set { _password = value?.Trim(); }
        }
    }
}
