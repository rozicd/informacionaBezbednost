using IB_projekat.Certificates.Model;
using IB_projekat.Users.Model;

namespace IB_projekat.Certificates.Service
{
    public interface ICertificateService
    {
        Task<IEnumerable<Certificate>> GetAll();
        Task<bool> VerifyCertificate(string certificateSerialNumber);
        Task<Certificate> IssueCertificate(string? issuerSN, User user, string keyUsageFlags, DateTime validTo);
    }
}
