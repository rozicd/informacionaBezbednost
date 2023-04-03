using IB_projekat.Users.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IB_projekat.Users.Repository
{
    public class UserRepository<T> : IUserRepository<T> where T : User
    {
        private readonly DbSet<T> _users;
        private readonly DatabaseContext _context;

        public UserRepository(DatabaseContext context)
        {
            _context = context;
            _users = context.Set<T>();
        }

        public T GetByEmailAndPassword(string email, string password)
        {
            return _users.FirstOrDefault(u => u.Email == email && u.Password == password);
        }

        public T GetById(int id)
        {
            return _users.FirstOrDefault(u => u.Id == id);
        }

        public IEnumerable<T> GetAll()
        {
            return _users.ToList();
        }

        public async Task Add(T user)
        {
            await _users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task Update(T user)
        {
            _users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(T user)
        {
            _users.Remove(user);
            await _context.SaveChangesAsync();
        }
    }
}