using IB_projekat.Requests.Model;
using IB_projekat.Requests.Model.Repository;
using Microsoft.EntityFrameworkCore;

namespace IB_projekat.Requests.Repository
{
    public class RequestRepository : IRequestRepository
    {
        private readonly DatabaseContext _context;

        public RequestRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task Add(Request request)
        {
            var user = await _context.Users.FindAsync(request.User.Id);
            request.User = _context.Entry(user).IsKeySet ? user : _context.Users.Attach(user).Entity;
            await _context.Requests.AddAsync(request);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Request>> GetAll(int page, int pageSize)
        {
            return await _context.Requests
                .Include(r => r.User)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

        }
        public async Task<int> GetRequestsCount()
        {
            return await _context.Requests.CountAsync();
        }

        public async Task<int> GetTotalCountForUser(int userId)
        {
            return await _context.Requests
                .Where(r => r.User.Id == userId)
                .CountAsync();
        }
        public async Task<Request> GetById(int id)
        {
            return await _context.Requests.Include(u=>u.User).FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<List<Request>> GetByUsersId(int id)
        {
            return await _context.Requests.Where(r => r.User.Id == id).ToListAsync();
        }

        public async Task<List<Request>> GetRequestsByCertificateSerialNumber(int userId, int page, int pageSize)
        {
            var certificates = await _context.Certificates
            .Where(c => c.User.Id == userId)
            .ToListAsync();

            var certificateSerialNumbers = certificates.Select(c => c.SerialNumber);

            var requestsQuery = _context.Requests
                .Include(r => r.User)
                .Where(r => certificateSerialNumbers.Contains(r.SignitureSerialNumber));

            var totalItems = await requestsQuery.CountAsync();

            var requests = await requestsQuery
                .OrderByDescending(r => r.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return requests;
        }

        public async Task Update(Request request)
        {
            _context.Requests.Update(request);
            await _context.SaveChangesAsync();
        }

    }
}
