using IB_projekat.Users.Model;
using IB_projekat.Users.Repository;

namespace IB_projekat.Users.Service
{
    public class UserService : IUserService
    {

        private readonly IUserRepository<User> _userRepository;

        public UserService(IUserRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task AddUser(DTOS.CreateUserDTO userDTO)
        {
            User user = new User();
            user.Name = userDTO.Name;
            user.Surname = userDTO.Surname;
            user.PhoneNumber = userDTO.PhoneNumber;
            user.Email = userDTO.Email;
            user.Role = UserType.Unauthorized;
            user.Password = userDTO.Password;
            await _userRepository.Add(user);
        }

        public async Task<User> Authenticate(string username, string password)
        {
           return await _userRepository.GetByEmailAndPassword(username, password);
            
        }

        public async Task<User> UpdateUser(int id, User user)
        {
            var existingUser = await _userRepository.GetById(id);
            if (existingUser == null)
            {
                return null;
            }
            existingUser.Email = user.Email;
            existingUser.Password = user.Password;
            await _userRepository.Update(existingUser);
            return existingUser;
        }
    }
}
