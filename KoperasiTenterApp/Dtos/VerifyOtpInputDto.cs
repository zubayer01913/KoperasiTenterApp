using System.ComponentModel.DataAnnotations;

namespace KoperasiTenterApp.Dtos
{
    public class VerifyOtpInputDto
    {
        public string? MobileNumber { get; set; }
        public string? EmailAddress { get; set; }

        [StringLength(4, MinimumLength = 4, ErrorMessage = "Code must be exactly 4 characters long.")]
        public string Code { get; set; }
    }
}
