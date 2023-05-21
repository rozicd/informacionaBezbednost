using System.ComponentModel.DataAnnotations;

namespace IB_projekat.Users.DTOS
{
    public class CreateUserDTO
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Surname { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public VerificationMethodType VerificationMethod { get; set; }

        [Required]
        public string RecaptchaToken { get; set; }

    }


    public enum VerificationMethodType
    {
        Email,
        PhoneNumber
    }
}
