using System.ComponentModel.DataAnnotations;

namespace IB_projekat.Users.DTOS
{
    public class UserEmailDTO
    {
        [Required]
        public string email { get; set; }
    }
}
