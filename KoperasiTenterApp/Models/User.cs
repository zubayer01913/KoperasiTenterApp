namespace KoperasiTenterApp.Models
{
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string CustomerName { get; set; }
        public string ICNumber { get; set; }
        public string MobileNumber { get; set; }
        public string EmailAddress { get; set; }
        public byte[]? PinHash { get; set; }
        public byte[]? PinSalt { get; set; }
        public bool IsBiometricEnabled { get; set; }
        public string? OtpCode { get; set; }
        public DateTime? OtpExpiresAt { get; set; }
        public bool IsMobileVerified { get; set; } = false;
        public bool IsEmailVerified { get; set; } = false;
        public bool AcceptedPrivacyPolicy { get; set; } = false;
    }
}


