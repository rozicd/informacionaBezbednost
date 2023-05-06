using IB_projekat.PasswordResetTokens.Model;
using Microsoft.EntityFrameworkCore;

namespace IB_projekat.PasswordResetTokens.Repository
{
    public class PasswordResetTokenRepository : IPasswordResetTokenRepository
    {
        private readonly DatabaseContext _dbContext;

        public PasswordResetTokenRepository(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddToken(PasswordResetToken token)
        {
            await _dbContext.Set<PasswordResetToken>().AddAsync(token);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<PasswordResetToken> GetTokenByToken(string token)
        {
            return await _dbContext.Set<PasswordResetToken>().SingleOrDefaultAsync(t => t.Token == token);
        }

        public async Task DeleteToken(PasswordResetToken token)
        {
            _dbContext.Set<PasswordResetToken>().Remove(token);
            await _dbContext.SaveChangesAsync();
        }
    }
}
