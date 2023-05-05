using IB_projekat.Certificates.Model;
using System.Collections.Generic;
using System.Linq;

using Microsoft.EntityFrameworkCore;

namespace IB_projekat.Certificates.Repository
{
    public class CertificateRepository : ICertificateRepository
    {
        private readonly DatabaseContext _context;
        private readonly DbSet<Certificate> _certs;

        public CertificateRepository(DatabaseContext context)
        {
            _context = context;
            _certs = context.Set<Certificate>();
        }

        public Certificate GetById(int id)
        {
            return _context.Certificates.FirstOrDefault(c => c.Id == id);
        }

        public async Task<IEnumerable<Certificate>> GetAll()
        {
            return await  _context.Certificates.ToListAsync();
        }

        public async Task<IEnumerable<Certificate>> GetAllEndCertificates()
        {
            return await _context.Certificates.Where(c => c.CertificateType == CertificateType.End).ToListAsync();
        }

        public async Task<IEnumerable<Certificate>> GetAllIntermediateCertificates()
        {
            return await _context.Certificates.Where(c => c.CertificateType == CertificateType.Intermediate).ToListAsync();
        }

        public async Task Add(Certificate certificate)
        {
            var user = await _context.Users.FindAsync(certificate.User.Id); 
            certificate.User = _context.Entry(user).IsKeySet ? user : _context.Users.Attach(user).Entity;
            await _context.Certificates.AddAsync(certificate);
            await _context.SaveChangesAsync();

        }

        public async Task Update(Certificate certificate)
        {
            _context.Certificates.Update(certificate);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(Certificate certificate)
        {
            _context.Certificates.Remove(certificate);
            await _context.SaveChangesAsync();
        }

        public async Task<Certificate> GetBySerialNumber(string serialNumber)
        {
            return await _context.Certificates.Include(c => c.User).FirstOrDefaultAsync(c => c.SerialNumber == serialNumber);
        }

        public async Task<List<Certificate>> GetAllCertificatesPaginated(int page, int pageSize)
        {
            return await _context.Certificates
                .Include(c => c.User)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
    }
}