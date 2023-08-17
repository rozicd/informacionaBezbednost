using System.ComponentModel.DataAnnotations;

namespace IB_projekat.Users.DTOS
{
    public class CreateUserDTO
    {
        [Required]
        [RegularExpression("^[A-Za-z]{1,30}$", ErrorMessage = "Name should contain only letters (max 30 characters).")]
        public string Name { get; set; }

        [Required]
        [RegularExpression("^[A-Za-z]{1,30}$", ErrorMessage = "Surname should contain only letters (max 30 characters).")]
        public string Surname { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$", ErrorMessage = "Password should contain at least 8 characters, including uppercase, lowercase, digit, and special character.")]
        public string Password { get; set; }

        [Required]
        [RegularExpression(@"^\+?[1-9]\d{1,14}$", ErrorMessage = "Phone number should be a valid international phone number.")]
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
