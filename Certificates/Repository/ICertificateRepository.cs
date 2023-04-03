using IB_projekat.Certificates.Model;

namespace IB_projekat.Certificates.Repository
{
    public interface ICertificateRepository
    {
        Certificate GetById(int id);
        IEnumerable<Certificate> GetAll();
        IEnumerable<Certificate> GetAllEndCertificates();
        IEnumerable<Certificate> GetAllIntermediateCertificates();
        void Add(Certificate certificate);
        void Update(Certificate certificate);
        void Delete(Certificate certificate);
    }
}
