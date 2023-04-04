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
            await _context.Requests.AddAsync(request);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Request>> GetAll()
        {
            return await _context.Requests.ToListAsync();
        }

        public async Task<Request> GetById(int id)
        {
            return await _context.Requests.FirstOrDefaultAsync(r => r.Id == id);

        }

        public async Task<IEnumerable<Request>> GetByUserId(int id)
        {
            return await _context.Requests.Where(r => r.User.Id == id).ToListAsync();
        }

        public async Task Update(Request request)
        {
            _context.Requests.Update(request);
            await _context.SaveChangesAsync();
        }

        Task<Request> IRequestRepository.GetById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
