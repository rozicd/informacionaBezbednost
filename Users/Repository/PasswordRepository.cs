using IB_projekat.Users.Model;
using Microsoft.EntityFrameworkCore;

namespace IB_projekat.Users.Repository
{
    public class PasswordRepository : IPasswordRepository

    {
        private readonly DbSet<Password> _passwords;
        private readonly DatabaseContext _context;

        public PasswordRepository(DatabaseContext context)
        {
            _context = context;
            _passwords = context.Set<Password>();
        }
        public async Task Add(Password password)
        {
            var user = await _context.Users.FindAsync(password.User.Id);
            password.User = _context.Entry(user).IsKeySet ? user : _context.Users.Attach(user).Entity;
            await _passwords.AddAsync(password);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(Password password)
        {
             _passwords.Remove(password);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Password>> GetByUserId(int id)
        {
            return await _passwords.Where(p => p.User.Id == id).ToListAsync();
        }

        public async Task<Password> GetByUserIdAndIsActive(int id)
        {
            return await _passwords.FirstOrDefaultAsync(p => p.User.Id == id && p.PasswordStatus == PasswordStatus.ACTIVE);
        }

        public async Task<Password> GetByUserIdAndPassword(int id, string password)
        {
            return await _passwords.FirstOrDefaultAsync(p => p.User.Id == id && p.DbPassword == password);
        }

        public async Task Update(Password password)
        {
             _passwords.Update(password);
            await _context.SaveChangesAsync();
        }
    }
}
