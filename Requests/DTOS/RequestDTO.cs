using IB_projekat.Certificates.Model;
using IB_projekat.Users.Model;
using System.ComponentModel.DataAnnotations;

namespace IB_projekat.Certificates.DTOS
{
    public class RequestDTO
    {
        public class ValidFlagsAttribute : ValidationAttribute
        {
            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                if (value != null)
                {
                    string flags = value.ToString();
                    string[] flagValues = flags.Split(',');

                    foreach (string flagValue in flagValues)
                    {
                        if (!int.TryParse(flagValue, out int flag))
                        {
                            return new ValidationResult("Invalid flag value format. Must be 'number,number,number'.");
                        }
                    }

                    return ValidationResult.Success;
                }

                return new ValidationResult("Flags field is required.");
            }
        }


        [Required]
        public CertificateType CertificateType { get; set; }

        public string SignitureSerialNumber { get; set; }
        
        [Required]
        public int UserId { get; set; }

        [ValidFlags]
        public string Flags { get; set; }

        [Required]
        public string RecaptchaToken { get; set; }
    }
}
