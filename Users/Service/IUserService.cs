using IB_projekat.PasswordResetTokens.Model;
using IB_projekat.Users.Model;

namespace IB_projekat.Users.Service
{
    public interface IUserService
    {
        Task AddUser(DTOS.CreateUserDTO user);
        Task<User> UpdateUser(int id, User user);
        Task<PasswordStatus> Authenticate(string username, string password);
        Task<bool> UserExists(string email);
        Task<User> GetById(int id);
        Task DeleteUser(int id);
        Task<User> GetByEmail(string email);
        Task SendPasswordResetEmail(User user, PasswordResetToken token);
        Task<bool> ResetUserPassword(int id, User user, string newPassword);
    }
}
