namespace KoperasiTenterApp.Dtos
{
    public class UserOutputDto
    {
        public Guid UserId { get; set; }
        public string MobileNumber { get; set; }
        public string EmailAddress { get; set; }
        public string? Message { get; set; }
        public bool IsMobileVerified { get; set; }
        public bool IsEmailVerified { get; set; }
    }
}
