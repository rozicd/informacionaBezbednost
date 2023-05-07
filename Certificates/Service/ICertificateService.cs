using IB_projekat.Certificates.Model;
using IB_projekat.Users.Model;
using System.Security.Cryptography.X509Certificates;

namespace IB_projekat.Certificates.Service
{
    public interface ICertificateService
    {
        Task<IEnumerable<Certificate>> GetAll();
        Task<bool> VerifyCertificate(string certificateSerialNumber);
        Task<Certificate> IssueCertificate(string? issuerSN, User user, string keyUsageFlags, DateTime validTo,CertificateType type);
        Task<bool> ValidateCert(string serialNumber);
        Task<bool> ValidateCertFile(X509Certificate2 certificateBytes);
        Task<bool> RevokeCert(string serialNumber);

        Task<List<Certificate>> GetAllCertificatesPaginated(int page, int pageSize);
    }
}
