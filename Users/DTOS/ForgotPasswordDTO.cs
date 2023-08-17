using System.ComponentModel.DataAnnotations;

namespace IB_projekat.Users.DTOS
{
    public class ForgotPasswordDTO
    {
        [Required]
        public string Email { get; set; }


        [Required]
        public string RecaptchaToken { get; set; }

    }
}
