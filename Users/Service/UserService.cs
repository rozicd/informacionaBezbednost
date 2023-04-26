using IB_projekat.Users.Model;
using IB_projekat.Users.Repository;
using SendGrid;
using SendGrid.Helpers.Mail;

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
                await Execute();

            
        }


        public async Task<User> Authenticate(string username, string password)
        {
           return await _userRepository.GetByEmailAndPassword(username, password);
            
        }

        static async Task Execute()
        {
            var apiKey = Environment.GetEnvironmentVariable("SEND_GRID_API_KEY");
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("ibprojekat@gmail.com", "IB support");
            var subject = "Usluge pusenja kurca";
            var to = new EmailAddress("sanduzicro19@gmail.com", "Example User");
            var plainTextContent = "mnogo pusi";
            var htmlContent = File.ReadAllText("Resources/accountActivation.html"); ;
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
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

        public async Task<bool> UserExists(string email)
        {
            User user = await _userRepository.GetByEmail(email);
            if (user == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
