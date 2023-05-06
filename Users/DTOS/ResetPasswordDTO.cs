using System.ComponentModel.DataAnnotations;

namespace IB_projekat.Users.DTOS
{
    public class ResetPasswordDTO
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Token { get; set; }

        [Required]
        public string NewPassword { get; set; }
    }
}
