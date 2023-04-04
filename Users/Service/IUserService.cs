using IB_projekat.Users.Model;

namespace IB_projekat.Users.Service
{
    public interface IUserService
    {
        Task AddUser(DTOS.CreateUserDTO user);
        Task<User> UpdateUser(int id, User user);
        Task<User> Authenticate(string username, string password);

    }
}
