using SYSTEMATIC.INFRASTRUCTURE.Responses;

namespace SYSTEMATIC.INFRASTRUCTURE.Requests
{
    public class ChangePasswordRequest : IRequest<ChangePasswordResponse>
    {
        private string _oldpassword;
        private string _newPassword;

        public string OldPassword
        {
            get => _oldpassword;
            set => _oldpassword = value?.Trim();
        }

        public string NewPassword
        {
            get => _newPassword;
            set => _newPassword = value?.Trim();
        }
    }
}
