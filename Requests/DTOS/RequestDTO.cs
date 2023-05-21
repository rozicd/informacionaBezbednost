using IB_projekat.Certificates.Model;
using IB_projekat.Users.Model;
using System.ComponentModel.DataAnnotations;

namespace IB_projekat.Certificates.DTOS
{
    public class RequestDTO
    {
        [Required]
        public CertificateType CertificateType { get; set; }

        public string SignitureSerialNumber { get; set; }
        
        [Required]
        public int UserId { get; set; }
        [Required]
        public string Flags { get; set; }

        [Required]
        public string RecaptchaToken { get; set; }
    }
}
