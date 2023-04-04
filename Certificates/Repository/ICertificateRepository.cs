using IB_projekat.Certificates.Model;

namespace IB_projekat.Certificates.Repository
{
    public interface ICertificateRepository
    {
        Certificate GetById(int id);
        Certificate GetBySerialNumber(string serialNumber);
        Task<IEnumerable<Certificate>> GetAll();
        Task<IEnumerable<Certificate>> GetAllEndCertificates();
        Task<IEnumerable<Certificate>> GetAllIntermediateCertificates();
        Task Add(Certificate certificate);
        Task Update(Certificate certificate);
        Task Delete(Certificate certificate);
    }
}
