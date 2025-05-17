using System.ComponentModel.DataAnnotations;

namespace KoperasiTenterApp.Dtos
{
    public class RegisterInputDto
    {
        public string CustomerName { get; set; }


        [MinLength(6, ErrorMessage = "IC Number must be at least 6 characters long.")]
        public string ICNumber { get; set; }

        [MinLength(11, ErrorMessage = "Mobile Number must be at least 11 characters long.")]
        public string MobileNumber { get; set; }

        [EmailAddress]
        public string EmailAddress { get; set; }
    }
}
