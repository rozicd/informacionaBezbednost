using IB_projekat.ActivationTokens.Model;
using IB_projekat.ActivationTokens.Service;
using IB_projekat.PasswordResetTokens.Model;
using IB_projekat.SmsVerification.Model;
using IB_projekat.SmsVerification.Service;
using IB_projekat.Users.Model;
using IB_projekat.Users.Repository;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Security.Cryptography;
using System.Text;
using Twilio;
using Twilio.Rest.Api.V2010.Account;


namespace IB_projekat.Users.Service
{
    public class UserService : IUserService
    {

        private readonly IUserRepository<User> _userRepository;
        private readonly IActivationTokenService _activationTokenService;
        private readonly ISmsVerificationService _smsVerificationService;
        private readonly IPasswordRepository _passwordRepository;

        public UserService(IUserRepository<User> userRepository, IActivationTokenService activationTokenService, ISmsVerificationService smsVerificationService,IPasswordRepository passwordRepository)
        {
            _userRepository = userRepository;
            _activationTokenService = activationTokenService;
            _smsVerificationService = smsVerificationService;
            _passwordRepository = passwordRepository;
        }

        public async Task AddUser(DTOS.CreateUserDTO userDTO)
        {
            User user = new User();
            user.Name = userDTO.Name;
            user.Surname = userDTO.Surname;
            user.PhoneNumber = userDTO.PhoneNumber;
            user.Email = userDTO.Email;
            user.Role = UserType.Unauthorized;

            // Hash the password using SHA-256 algorithm
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashedPassword = sha256.ComputeHash(Encoding.UTF8.GetBytes(userDTO.Password));
                user.Password = Convert.ToBase64String(hashedPassword);
            }

            await _userRepository.Add(user);
            Password password = new Password();
            password.DbPassword = user.Password;
            password.User = user;
            password.ExpirationDate = DateTime.Now.AddDays(30);
            password.PasswordStatus = PasswordStatus.ACTIVE;
            _passwordRepository.Add(password);


            if (userDTO.VerificationMethod == DTOS.VerificationMethodType.Email)
            {
                ActivationToken token = await CreateActivationToken(user.Id);
                await SendActivationEmail(user, token);
            }
            else
            {
                SmsVerificationCode code = await _smsVerificationService.GenerateCode(user.Id, VerificationType.VERIFICATION);
                await SendActivationSMS(user, code);
            }



        }


        public async Task<PasswordStatus> Authenticate(string email, string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashedPassword = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                string hashedPasswordString = Convert.ToBase64String(hashedPassword);

                User user = await _userRepository.GetByEmail(email);

                if (user == null)
                {
                    return PasswordStatus.INACTIVE;
                }
                Password pass = await _passwordRepository.GetByUserIdAndPassword(user.Id, hashedPasswordString);
                if (pass == null)
                {
                    return PasswordStatus.INACTIVE;
                }

                if (pass.PasswordStatus.Equals(PasswordStatus.ACTIVE))
                {
                    if (pass.ExpirationDate < DateTime.Now)
                    {
                        pass.PasswordStatus = PasswordStatus.EXPIRED;
                        await _passwordRepository.Update(pass);
                    }
                }
                return pass.PasswordStatus;
                

            }
        }

        private async Task<ActivationToken> CreateActivationToken(int userId)
        {
            return await _activationTokenService.GenerateToken(userId);
        }

        static async Task SendActivationSMS(User user,SmsVerificationCode code)
        {
            string accountSid = Environment.GetEnvironmentVariable("TWILLIO_SMS_ACCOUNT_ID");
            string authToken = Environment.GetEnvironmentVariable("TWILLIO_SMS_AUTH_TOKEN");

            TwilioClient.Init(accountSid, authToken);

            var message = MessageResource.Create(
                body: "This is your certificate app verification code:\n\n"+code.Code+"\n\nTeam 23 IB",
                from: new Twilio.Types.PhoneNumber("+13203616935"),
                statusCallback: new Uri("http://postb.in/1234abcd"),
                to: new Twilio.Types.PhoneNumber(user.PhoneNumber)
            );

            Console.WriteLine(message.Sid);
        }
        static async Task SendActivationEmail(User user, ActivationToken token)
        {

            var apiKey = Environment.GetEnvironmentVariable("SEND_GRID_API_KEY");
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("ibprojekat@gmail.com", "IB support");
            var subject = "Test";
            var to = new EmailAddress(user.Email, "Example User");
            var plainTextContent = "test";
            var htmlContent = File.ReadAllText("Resources/accountActivation.html");
            var newHtmlContent = htmlContent.Replace("{{action_url}}", "http://localhost:3000/activate?id="+user.Id+"&token="+token.value);

            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, newHtmlContent);
            var response = await client.SendEmailAsync(msg);
        }

        public async Task SendPasswordResetEmail(User user, PasswordResetToken token)
        {

            var apiKey = Environment.GetEnvironmentVariable("SEND_GRID_API_KEY");
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("ibprojekat@gmail.com", "IB support");
            var subject = "Test";
            var to = new EmailAddress(user.Email, "Example User");
            var plainTextContent = "test";
            var htmlContent = File.ReadAllText("Resources/forgotPassword.html");
            var newHtmlContent = htmlContent.Replace("{{action_url}}", "http://localhost:3000/reset-password?id=" + user.Id + "&token=" + token.Token);

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

        public async Task DeleteUser(int id)
        {
            var user = await _userRepository.GetById(id);
            if (user != null)
            {
                await _userRepository.Delete(user);
            }
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
        
         public async Task<User> GetByEmail(string email)
        {
            return await _userRepository.GetByEmail(email);
        }

        public async Task<bool> ResetUserPassword(int id, User user, string newPassword)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashedPassword = sha256.ComputeHash(Encoding.UTF8.GetBytes(newPassword));
                string hashedPasswordString = Convert.ToBase64String(hashedPassword);

                user.Password = hashedPasswordString;
                IEnumerable<Password> password = await _passwordRepository.GetByUserId(user.Id);
                foreach (Password p in password)
                {
                    if (hashedPasswordString.Equals(p.DbPassword))
                    {
                        return false;
                    }
        
                }
                foreach (Password p in password)
                {
                    p.PasswordStatus = PasswordStatus.INACTIVE;
                    await _passwordRepository.Update(p);

                }
                Password newPass = new Password();
                newPass.DbPassword = hashedPasswordString;
                newPass.PasswordStatus = PasswordStatus.ACTIVE;
                newPass.ExpirationDate = DateTime.Now.AddDays(30);
                newPass.User = user;
                await _passwordRepository.Add(newPass);
                await UpdateUser(id, user);
                return true;

            }
        }

        public async Task<bool> TwoFactorAuthentication(string email)
        {
            User user = await _userRepository.GetByEmail(email);

            SmsVerificationCode code = await _smsVerificationService.GenerateCode(user.Id,VerificationType.TWO_FACTOR);
            await Send2FA(user, code);

            return true;

        }

        static async Task Send2FA(User user, SmsVerificationCode code)
        {
            var apiKey = Environment.GetEnvironmentVariable("SEND_GRID_API_KEY");
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("ibprojekat@gmail.com", "IB support");
            var subject = "Test";
            var to = new EmailAddress(user.Email, "Example User");
            var plainTextContent = "test";
            var htmlContent = File.ReadAllText("Resources/2fa.html");
            var newHtmlContent = htmlContent.Replace("{{action_url}}",code.Code);

            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, newHtmlContent);
            var response = await client.SendEmailAsync(msg);
        }

        public async Task<bool> TwoFactorAuthenticationSMS(string email)
        {
            User user = await _userRepository.GetByEmail(email);

            SmsVerificationCode code = await _smsVerificationService.GenerateCode(user.Id, VerificationType.TWO_FACTOR);
            SendActivationSMS(user, code);
            return true;

        }
    }
}
