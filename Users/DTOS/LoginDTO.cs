using System.ComponentModel.DataAnnotations;

namespace IB_projekat.Users.DTOS
{
    public class LoginDTO
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
