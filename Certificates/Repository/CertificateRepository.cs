using IB_projekat.Certificates.Model;
using System.Collections.Generic;
using System.Linq;

namespace IB_projekat.Certificates.Repository
{
    public class CertificateRepository : ICertificateRepository
    {
        private readonly DatabaseContext _context;

        public CertificateRepository(DatabaseContext context)
        {
            _context = context;
        }

        public Certificate GetById(int id)
        {
            return _context.Certificates.FirstOrDefault(c => c.Id == id);
        }

        public IEnumerable<Certificate> GetAll()
        {
            return _context.Certificates.ToList();
        }

        public IEnumerable<Certificate> GetAllEndCertificates()
        {
            return _context.Certificates.Where(c => c.CertificateType == CertificateType.End).ToList();
        }

        public IEnumerable<Certificate> GetAllIntermediateCertificates()
        {
            return _context.Certificates.Where(c => c.CertificateType == CertificateType.Intermediate).ToList();
        }

        public void Add(Certificate certificate)
        {
            _context.Certificates.Add(certificate);
            _context.SaveChanges();
        }

        public void Update(Certificate certificate)
        {
            _context.Certificates.Update(certificate);
            _context.SaveChanges();
        }

        public void Delete(Certificate certificate)
        {
            _context.Certificates.Remove(certificate);
            _context.SaveChanges();
        }
    }
}