using System.ComponentModel.DataAnnotations;

namespace KoperasiTenterApp.Dtos
{
    public class SetPinInputDto
    {
        public Guid UserId { get; set; }

        [StringLength(6, MinimumLength = 6, ErrorMessage = "Pin must be exactly 6 characters long.")]
        public string Pin { get; set; }
        public bool EnableBiometric { get; set; }
        public bool AcceptedPrivacyPolicy { get; set; }
    }
}
