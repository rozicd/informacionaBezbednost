using IB_projekat.ActivationTokens.DTOS;
using IB_projekat.ActivationTokens.Model;
using Microsoft.EntityFrameworkCore;

namespace IB_projekat.ActivationTokens.Repository
{
    public class ActivationTokenRepository : IActivationTokenRepository
    {
        private readonly DatabaseContext _context;

        public ActivationTokenRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task AddOne(ActivationToken activationToken)
        {
            await _context.ActivationTokens.AddAsync(activationToken);
            await _context.SaveChangesAsync();

        }

        public async Task DeleteOne(ActivationToken activationToken)
        {
            _context.ActivationTokens.Remove(activationToken);
            await _context.SaveChangesAsync();
        }

        public async Task<List<ActivationToken>> GetByUserId(int id)
        {
            return await _context.ActivationTokens.Where(r => r.userId == id).ToListAsync();
        }

    }
}
