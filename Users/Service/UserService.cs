using IB_projekat.ActivationTokens.Model;
using IB_projekat.ActivationTokens.Service;
using IB_projekat.Users.Model;
using IB_projekat.Users.Repository;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace IB_projekat.Users.Service
{
    public class UserService : IUserService
    {

        private readonly IUserRepository<User> _userRepository;
        private readonly IActivationTokenService _activationTokenService;

        public UserService(IUserRepository<User> userRepository, IActivationTokenService activationTokenService)
        {
            _userRepository = userRepository;
            _activationTokenService = activationTokenService;
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
            ActivationToken token = await CreateActivationToken(user.Id);
            await SendActivationEmail(user, token);
            



        }


        public async Task<User> Authenticate(string username, string password)
        {
           return await _userRepository.GetByEmailAndPassword(username, password);
            
        }

        private async Task<ActivationToken> CreateActivationToken(int userId)
        {
            return await _activationTokenService.GenerateToken(userId);
        }

        static async Task SendActivationEmail(User user, ActivationToken token)
        {

            var apiKey = Environment.GetEnvironmentVariable("SEND_GRID_API_KEY");
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("ibprojekat@gmail.com", "IB support");
            var subject = "Test";
            var to = new EmailAddress("sanduzicro19@gmail.com", "Example User");
            var plainTextContent = "test";
            var htmlContent = File.ReadAllText("Resources/accountActivation.html");
            var newHtmlContent = htmlContent.Replace("{{action_url}}", "http://localhost:3000/activate?id="+user.Id+"&token="+token.value);

            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, newHtmlContent);
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
            existingUser.Role = user.Role;
            existingUser.PhoneNumber = user.PhoneNumber;
            existingUser.Surname = user.Surname;
            existingUser.Name = user.Name;
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

        public async Task<User> GetById(int id)
        {
            return await _userRepository.GetById(id);
        }
    }
}
