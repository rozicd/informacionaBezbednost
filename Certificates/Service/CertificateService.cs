using IB_projekat.Certificates.Model;
using IB_projekat.Certificates.Repository;

namespace IB_projekat.Certificates.Service
{
    public class CertificateService : ICertificateService
    {
        private readonly ICertificateRepository _certificateRepository;

        public CertificateService(ICertificateRepository certificateRepository)
        {
            _certificateRepository = certificateRepository;
        }

        public async Task<IEnumerable<Certificate>> GetAll()
        {
            return await _certificateRepository.GetAll();
        }

        public Task<bool> VerifyCertificate(string certificateSerialNumber)
        {
            throw new NotImplementedException();
        }
    }
}
