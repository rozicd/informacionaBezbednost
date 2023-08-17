using IB_projekat.Users.Model;

namespace IB_projekat.Users.Repository
{
    public interface IPasswordRepository
    {
        Task<IEnumerable<Password>> GetByUserId(int id);
        Task<Password> GetByUserIdAndPassword(int id,string password);
        Task<Password> GetByUserIdAndIsActive(int id);
        Task Add(Password password);
        Task Update(Password password);
        Task Delete(Password password);

    }
}
