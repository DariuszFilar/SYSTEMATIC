namespace SYSTEMATIC.DB.Entities
{
    public class User
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }
        public string EmailVerificationCode { get; set; }
        public DateTime? EmailVerificationCodeExpireAt { get; set; }
        public DateTime EmialVerificationCodeSendedAt { get; set; }
    }
}
