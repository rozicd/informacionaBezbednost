using IB_projekat.Users.Model;

namespace IB_projekat.Users.Repository
{
    public interface IUserRepository<T> where T : User
    {
        T GetByEmailAndPassword(string email, string password);
        IEnumerable<T> GetAll();
        public T GetById(int id);
        Task Add(T user);
        Task Update(T user);
        Task Delete(T user);
    }
}
