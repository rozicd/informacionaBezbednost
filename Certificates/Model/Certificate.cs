using IB_projekat.Users.Model;

namespace IB_projekat.Certificates.Model
{
    public class Certificate
    {
        public int Id { get; set; }
        public string SerialNumber { get; set; }
        public string SignatureAlgorithm { get; set; }
        public string? Issuer { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public CertificateStatus Status { get; set; }
        public CertificateType CertificateType { get; set; }
        public User User { get; set; }
    }

    public enum CertificateStatus
    {
        Valid,
        NotValid,
        Revoked
    }

    public enum CertificateType
    {
        Root,
        Intermediate,
        End
    }
}
