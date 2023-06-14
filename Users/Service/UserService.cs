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
        private readonly Serilog.ILogger _logger;

        public UserService(Serilog.ILogger logger,IUserRepository<User> userRepository, IActivationTokenService activationTokenService, ISmsVerificationService smsVerificationService,IPasswordRepository passwordRepository)
        {
            _logger = logger;
            _userRepository = userRepository;
            _activationTokenService = activationTokenService;
            _smsVerificationService = smsVerificationService;
            _passwordRepository = passwordRepository;
        }

        public async Task AddOAuthUser(string email,string name,string surname)
        {
            User user = new User();
            user.Name = name;
            user.Surname = surname;
            user.PhoneNumber = "";
            user.Email = email;
            user.IsOAuth = true;
            user.Role = UserType.Authorized;
            _logger.Information("Adding OAuth user: {Email}, Name: {Name}, Surname: {Surname}", user.Email, user.Name, user.Surname);

            await _userRepository.Add(user);


        }
        public async Task AddUser(DTOS.CreateUserDTO userDTO)
        {
            User user = new User();
            user.Name = userDTO.Name;
            user.Surname = userDTO.Surname;
            user.PhoneNumber = userDTO.PhoneNumber;
            user.Email = userDTO.Email;
            user.Role = UserType.Unauthorized;

            string passwordString = null; 
            // Hash the password using SHA-256 algorithm
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashedPassword = sha256.ComputeHash(Encoding.UTF8.GetBytes(userDTO.Password));
                passwordString = Convert.ToBase64String(hashedPassword);
            }

            await _userRepository.Add(user);
            Password password = new Password();
            password.DbPassword = passwordString;
            password.User = user;
            password.ExpirationDate = DateTime.Now.AddDays(30);
            password.PasswordStatus = PasswordStatus.ACTIVE;
            user.IsOAuth = false;
            _passwordRepository.Add(password);


            if (userDTO.VerificationMethod == DTOS.VerificationMethodType.Email)
            {
                ActivationToken token = await CreateActivationToken(user.Id);
                await SendActivationEmail(user, token);
                _logger.Information("Activation token sent via email to user: {Email}", user.Email);
            }
            else
            {
                SmsVerificationCode code = await _smsVerificationService.GenerateCode(user.Id, VerificationType.VERIFICATION);
                await SendActivationSMS(user, code);
                _logger.Information("Activation code sent via SMS to user: {PhoneNumber}", user.PhoneNumber);
            }
            _logger.Information("User {CurrentUser} added a new user: Email={Email}, Name={Name}, Surname={Surname}, PhoneNumber={PhoneNumber}, Role={Role}, VerificationMethod={VerificationMethod}",
                user.Email, user.Email, user.Name, user.Surname, user.PhoneNumber, user.Role, userDTO.VerificationMethod);


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
                    _logger.Warning("Authentication failed. User with email {Email} does not exist.", email);
                    return PasswordStatus.INACTIVE;
                }
                Password pass = await _passwordRepository.GetByUserIdAndPassword(user.Id, hashedPasswordString);
                if (pass == null)
                {
                    _logger.Warning("Authentication failed. Invalid password for user with email {Email}.", email);

                    return PasswordStatus.INACTIVE;
                }

                if (pass.PasswordStatus.Equals(PasswordStatus.ACTIVE))
                {
                    if (pass.ExpirationDate < DateTime.Now)
                    {
                        pass.PasswordStatus = PasswordStatus.EXPIRED;
                        _logger.Warning("Authentication failed. Password has expired for user with email {Email}.", email);

                        await _passwordRepository.Update(pass);
                    }
                }
                _logger.Information("Authentication successful for user with email {Email}.", email);

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
                body: "This is your certificate app verification code:\n\n" + code.Code + "\n\nTeam 23 IB",
                from: new Twilio.Types.PhoneNumber("+13203616935"),
                statusCallback: new Uri("http://postb.in/1234abcd"),
                to: new Twilio.Types.PhoneNumber("+381631684637")
            ) ;

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
            _logger.Information("Updating user with ID {UserId}.", id);
            var existingUser = await _userRepository.GetById(id);
            if (existingUser == null)
            {
                _logger.Warning("User with ID {UserId} does not exist. Update operation aborted.", id);
                return null;
            }
            existingUser.Email = user.Email;
            existingUser.Role = user.Role;
            existingUser.PhoneNumber = user.PhoneNumber;
            existingUser.Surname = user.Surname;
            existingUser.Name = user.Name;
            await _userRepository.Update(existingUser);
            _logger.Information("User with ID {UserId} updated successfully.", id);
            return existingUser;
        }

        public async Task DeleteUser(int id)
        {
            _logger.Information("Deleting user with ID {UserId}.", id);
            var user = await _userRepository.GetById(id);
            if (user != null)
            {   
                await _userRepository.Delete(user);
                _logger.Information("User with ID {UserId} deleted successfully.", id);
            }
            else
            {
                _logger.Warning("User with ID {UserId} does not exist. Deletion operation aborted.", id);
            }
        }

        public async Task<bool> UserExists(string email)
        {
            _logger.Information("Checking if user with email {Email} exists.", email);

            User user = await _userRepository.GetByEmail(email);

            if (user == null)
            {
                _logger.Information("User with email {Email} does not exist.", email);
                return false;
            }
            else
            {
                _logger.Information("User with email {Email} exists.", email);
                return true;
            }
        }

        public async Task<User> GetById(int id)
        {
            _logger.Information("Retrieving user by ID: {UserId}.", id);
            User user = await _userRepository.GetById(id);

            if (user == null)
            {
                _logger.Information("User with ID {UserId} does not exist.", id);
            }
            else
            {
                _logger.Information("User retrieved successfully. ID: {UserId}, Email: {UserEmail}.", user.Id, user.Email);
            }

            return user;
        }
        
         public async Task<User> GetByEmail(string email)
        {
            _logger.Information("Retrieving user by email: {UserEmail}.", email);
            User user = await _userRepository.GetByEmail(email);

            if (user == null)
            {
                _logger.Information("User with email {UserEmail} does not exist.", email);
            }
            else
            {
                _logger.Information("User retrieved successfully. ID: {UserId}, Email: {UserEmail}.", user.Id, user.Email);
            }

            return user;
        }

        public async Task<bool> ResetUserPassword(int id, User user, string newPassword)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashedPassword = sha256.ComputeHash(Encoding.UTF8.GetBytes(newPassword));
                string hashedPasswordString = Convert.ToBase64String(hashedPassword);

                IEnumerable<Password> passwords = await _passwordRepository.GetByUserId(user.Id);

                foreach (Password p in passwords)
                {
                    if (hashedPasswordString.Equals(p.DbPassword))
                    {
                        _logger.Warning("New password matches an existing past password for user with ID {UserId}. Reset operation aborted.", id);
                        return false;
                    }
                }

                foreach (Password p in passwords)
                {
                    p.PasswordStatus = PasswordStatus.INACTIVE;
                    await _passwordRepository.Update(p);
                    _logger.Information("Password with ID {PasswordId} set to INACTIVE for user with ID {UserId}.", p.Id, id);
                }

                Password newPass = new Password();
                newPass.DbPassword = hashedPasswordString;
                newPass.PasswordStatus = PasswordStatus.ACTIVE;
                newPass.ExpirationDate = DateTime.Now.AddDays(30);
                newPass.User = user;
                await _passwordRepository.Add(newPass);
                _logger.Information("New password added for user with ID {UserId}.", id);

                await UpdateUser(id, user);

                _logger.Information("Password reset successfully for user with ID {UserId}.", id);
                return true;
            }
        }

        public async Task<bool> TwoFactorAuthentication(string email)
        {
            User user = await _userRepository.GetByEmail(email);

            SmsVerificationCode code = await _smsVerificationService.GenerateCode(user.Id, VerificationType.TWO_FACTOR);
            await Send2FA(user, code);

            _logger.Information("Two-factor authentication code sent to user with email {UserEmail}.", email);

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

            _logger.Information("Two-factor authentication SMS sent to user with email {UserEmail}.", email);

            return true;

        }
    }
}
