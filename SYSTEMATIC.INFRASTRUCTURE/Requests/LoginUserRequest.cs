using SYSTEMATIC.INFRASTRUCTURE.Responses;

namespace SYSTEMATIC.INFRASTRUCTURE.Requests
{
    public class LoginUserRequest : IRequest<LoginUserResponse>
    {
        private string _email;
        private string _password;

        public string Email
        {
            get => _email;
            set => _email = value?.Trim();
        }

        public string Password
        {
            get => _password;
            set => _password = value?.Trim();
        }
    }
}
