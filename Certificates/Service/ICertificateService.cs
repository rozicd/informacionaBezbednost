using IB_projekat.Certificates.Model;

namespace IB_projekat.Certificates.Service
{
    public interface ICertificateService
    {
        Task<IEnumerable<Certificate>> GetAll();
        Task<bool> VerifyCertificate(string certificateSerialNumber);
    }
}
