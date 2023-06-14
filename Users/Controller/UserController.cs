using IB_projekat.Users.Model;
using IB_projekat.Users.Repository;
using IB_projekat.Users.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using IB_projekat.Users.DTOS;
using IB_projekat.ActivationTokens.Service;
using IB_projekat.ActivationTokens.Model;
using IB_projekat.SmsVerification.Service;
using IB_projekat.SmsVerification.Model;
using IB_projekat.PasswordResetTokens.Service;
using IB_projekat.PasswordResetTokens.Model;
using IB_projekat.tools;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Cors;
namespace IB_projekat.Users.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IActivationTokenService _activationTokenService;
        private readonly ISmsVerificationService _smsVerificationService;
        private readonly IPasswordResetTokenService _passwordResetTokenService;
        private readonly RecaptchaVerifier _recaptchaVerifier;
        private readonly Serilog.ILogger _logger;

        public UserController(Serilog.ILogger loger,IUserService userService, IActivationTokenService activationTokenService, ISmsVerificationService smsVerificationService, IPasswordResetTokenService passwordResetTokenService)
        {
            _logger = loger;
            _userService = userService;
            _activationTokenService = activationTokenService;
            _smsVerificationService = smsVerificationService;
            _passwordResetTokenService = passwordResetTokenService;
            _recaptchaVerifier = new RecaptchaVerifier(Environment.GetEnvironmentVariable("BACK_RECAPTCHA"));


        }

        [HttpPost("register")]
        public async Task<IActionResult> AddUser(DTOS.CreateUserDTO user)
        {
            if (!await _recaptchaVerifier.VerifyRecaptcha(user.RecaptchaToken))
            {
                _logger.Warning("Recaptcha verification failed for user registration: {Email}", user.Email);
                return BadRequest("Recaptcha is not valid!");
            }

            if (!_userService.UserExists(user.Email).Result)
            {
                await _userService.AddUser(user);
                return Ok();
            }
            else
            {
                return Conflict("USER WITH THIS EMAIL ALREADY EXISTS");
            }
            
        }

        [HttpPut("{id}")]
        [Microsoft.AspNetCore.Authorization.Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateUser(int id, User user)
        {
            var existingUser = await _userService.UpdateUser(id,user);
            if (existingUser == null)
            {
                return NotFound();
            }
            return Ok();
        }

        [Microsoft.AspNetCore.Authorization.AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO model)
        {
            _logger.Information("Korisnik je primio kurac u dupe");
            if (! await _recaptchaVerifier.VerifyRecaptcha(model.RecaptchaToken))
            {
                return BadRequest("Recaptcha is not valid!");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var passwordStatus = await _userService.Authenticate(model.Username, model.Password);
            if (passwordStatus == PasswordStatus.INACTIVE)
            {
                return Unauthorized();
            }
            User user = await _userService.GetByEmail(model.Username);
            if (passwordStatus == PasswordStatus.EXPIRED)
            {
                PasswordResetToken token = await _passwordResetTokenService.GenerateToken(user.Id);
                string redirectUrl = "http://localhost:3000/reset-password?id=" + user.Id + "&token=" + token.Token;
                return Ok(new { redirectUrl });
                
            }


            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email.ToString()),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
                new Claim("TwoFactorVerified", "false")
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity));
            
            return Ok(user);
        }




        [Microsoft.AspNetCore.Authorization.AllowAnonymous]
        [HttpGet("/api/user/google-login")]
        public IActionResult GoogleLogin()
        {

            string url = Url.Action("GoogleResponse");

            var properties = new AuthenticationProperties{ RedirectUri = "http://localhost:8000/api/user/google-response" };

            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        [HttpGet("/api/user/google-response")]
        public async Task<IActionResult> GoogleResponse()
        {
            var authenticateResult = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);
            if (!authenticateResult.Succeeded)
            {
                // Handle authentication failure
                return RedirectToAction("GoogleLogin");
            }

            string email = User.FindFirstValue(ClaimTypes.Email);
            string name = User.FindFirstValue(ClaimTypes.GivenName);
            string surname = User.FindFirstValue(ClaimTypes.Surname);

            if (await _userService.UserExists(email))
            {
                User user = await _userService.GetByEmail(email);
                if (user.IsOAuth)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.Email.ToString()),
                        new Claim(ClaimTypes.Role, user.Role.ToString()),
                        new Claim("TwoFactorVerified", "true")
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity));
                    return Redirect("http://localhost:3000/home");
                }
                else
                {
                    return BadRequest("Email already registered with a different method!");
                }
            }
            else
            {

                await _userService.AddOAuthUser(email, name, surname);
                var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, email),
                        new Claim(ClaimTypes.Role, UserType.Authorized.ToString()),
                        new Claim("TwoFactorVerified", "true")
                    };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity));
                return Redirect("http://localhost:3000/home");
            }
           

        }
        [HttpGet("authorized")]
        [Microsoft.AspNetCore.Authorization.Authorize(Policy = "AuthorizedOnly")]
        public IActionResult GetAuthorizedData()
        {
            // Only users with the Authorized or Admin role can access this action
            var userEmail = User.FindFirstValue(ClaimTypes.Name);
            if (User.FindFirstValue("TwoFactorVerified") == "false")
            {
                return Forbid();
            }
            return Ok(userEmail);
        }


        [HttpPut("activate/{id}/{token}")]
        public async Task<IActionResult> ActivateUserAsync(int id, string token)
        {

            User user = await _userService.GetById(id);
            if (user == null)
            {
                return NotFound();
            }
            List<ActivationToken> activationTokens = await _activationTokenService.getTokenByUserId(id);
            var validTokenFound = false;
            foreach (var activationToken in activationTokens)
            {
                if (activationToken.expires >= DateTime.Now && _activationTokenService.VerifyToken(token, activationToken.hash))
                {
                    validTokenFound = true;
                    break;
                }
            }
            if (!validTokenFound)
            {
                foreach (var activationToken in activationTokens)
                {
                    await _activationTokenService.RemoveToken(activationToken);
                }

                await _userService.DeleteUser(user.Id);
                return BadRequest("Invalid or expired activation token");
            }

            // Activate the user account
            user.Role = UserType.Authorized;
            await _userService.UpdateUser(user.Id, user);

            // Delete the activation token(s)
            foreach (var activationToken in activationTokens)
            {
                await _activationTokenService.RemoveToken(activationToken);
            }

            return Ok();
        }
        [HttpPost("2fa/activate/{code}")]
        public async Task<IActionResult> Activate2fa(string code)
        {
            SmsVerificationCode smsVerificationCode = _smsVerificationService.GetCodeByCodeValueAndType(code, VerificationType.TWO_FACTOR);
            if (smsVerificationCode == null)
            {
                return NotFound();
            }

            User user = await _userService.GetById(smsVerificationCode.UserId);
            if (user == null)
            {
                await _smsVerificationService.DeleteCode(smsVerificationCode);
                return NotFound();
            }

            List<SmsVerificationCode> smsCodes = await _smsVerificationService.GetCodesByUserId(smsVerificationCode.UserId);
            var validTokenFound = false;
            foreach (var smsCode in smsCodes)
            {
                if (smsCode.type.Equals(VerificationType.TWO_FACTOR) && smsCode.Expires >= DateTime.Now && _smsVerificationService.VerifyCode(code, smsCode.Code))
                {
                    validTokenFound = true;
                    break;
                }
            }
            if (!validTokenFound)
            {
                foreach (var smsCode in smsCodes)
                {
                    await _smsVerificationService.DeleteCode(smsCode);
                }

                
                return BadRequest("Invalid or expired sms activation token");
            }

           
            
            foreach (var smsCode in smsCodes)
            {
                await _smsVerificationService.DeleteCode(smsCode);
            }

            var claimsPrincipal = HttpContext.User as ClaimsPrincipal;
            var identity = claimsPrincipal.Identity as ClaimsIdentity;

            var existingClaim = identity.FindFirst("TwoFactorVerified");
            if (existingClaim != null)
            {
                identity.RemoveClaim(existingClaim);
            }

            identity.AddClaim(new Claim("TwoFactorVerified", "true"));

            await HttpContext.SignInAsync(claimsPrincipal);

           
            return Ok();
        }

       



            [HttpPost("activateSms/{code}")]
        public async Task<IActionResult> ActivateUserSms(string code)
        {
            SmsVerificationCode smsVerificationCode = _smsVerificationService.GetCodeByCodeValueAndType(code,VerificationType.VERIFICATION);
            if (smsVerificationCode == null)
            {
                return NotFound();
            }

            User user = await _userService.GetById(smsVerificationCode.UserId);
            if (user == null)
            {
                await _smsVerificationService.DeleteCode(smsVerificationCode);
                return NotFound();
            }

            List<SmsVerificationCode> smsCodes = await _smsVerificationService.GetCodesByUserId(smsVerificationCode.UserId);
            var validTokenFound = false;
            foreach (var smsCode in smsCodes)
            {
                if (smsCode.type.Equals(VerificationType.VERIFICATION) && smsCode.Expires >= DateTime.Now && _smsVerificationService.VerifyCode(code, smsCode.Code))
                {
                    validTokenFound = true;
                    break;
                }
            }
            if (!validTokenFound)
            {
                foreach (var smsCode in smsCodes)
                {
                    await _smsVerificationService.DeleteCode(smsCode);
                }

                await _userService.DeleteUser(user.Id);
                return BadRequest("Invalid or expired sms activation token");
            }

            // Activate the user account
            user.Role = UserType.Authorized;
            await _userService.UpdateUser(user.Id, user);

            // Delete the activation token(s)
            foreach (var smsCode in smsCodes)
            {
                await _smsVerificationService.DeleteCode(smsCode);
            }

            return Ok();
        }

        [HttpPost("forgotpassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDTO forgotPassword)
        {
            if (!await _recaptchaVerifier.VerifyRecaptcha(forgotPassword.RecaptchaToken))
            {
                return BadRequest("Recaptcha is not valid!");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var user = await _userService.GetByEmail(forgotPassword.Email);
            if (user == null)
            {
                return NotFound();
            }

            if (!user.IsOAuth)
            {
                var token = await _passwordResetTokenService.GenerateToken(user.Id);

                await _userService.SendPasswordResetEmail(user, token);

                return Ok();
            }
            else
            {
                return BadRequest("User is already registered with a different method!");
            }
        }

        [HttpPost("logout")]
        [Microsoft.AspNetCore.Authorization.Authorize(Policy = "AuthorizedOnly")]
        public async Task<IActionResult> Logout()       
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok();
        }

        [HttpPost("verify-password-reset-token/{token}")]
        public async Task<IActionResult> VerifyPasswordResetToken(string token)
        {
            var passwordResetToken = await _passwordResetTokenService.GetTokenByToken(token);

            if (passwordResetToken == null)
            {
                return NotFound();
            }

            if (passwordResetToken.ExpirationDate < DateTime.Now)
            {
                await _passwordResetTokenService.DeleteToken(passwordResetToken);
                return BadRequest("The password reset token has expired.");
            }

            return Ok("The password reset token is valid.");
        }

        [HttpPost("email")]
        public async Task<User> GetByEmail([FromBody] UserEmailDTO userEmailDTO)
        {
            User user = await _userService.GetByEmail(userEmailDTO.email);
            return user;
        }
        

        [HttpPost("2fa/{type}")]
        public async Task<IActionResult> TwoFactorAuthentication(string type)
        {

            string email = User.FindFirstValue(ClaimTypes.Name);
            if (type == "email")
            {
                bool twofa = await _userService.TwoFactorAuthentication(email);
            }
            else
            {
                bool twofa = await _userService.TwoFactorAuthenticationSMS(email);
            }

            return Ok();
        }


        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDTO model)
        {
            if (!await _recaptchaVerifier.VerifyRecaptcha(model.RecaptchaToken))
            {
                return BadRequest("Recaptcha is not valid!");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userService.GetById(model.Id);
            if (user == null)
            {
                return NotFound("Password reset is invalid");
            }

            var passwordResetToken = await _passwordResetTokenService.GetTokenByToken(model.Token);

            if (passwordResetToken == null)
            {
                return NotFound();
            }

            if (passwordResetToken.ExpirationDate < DateTime.Now)
            {
                await _passwordResetTokenService.DeleteToken(passwordResetToken);
                return BadRequest("The password reset token has expired.");
            }
            bool succes = await _userService.ResetUserPassword(user.Id, user, model.NewPassword);
            if (!succes) 
            {
                return BadRequest("You cannot use the same password!");
            }

            await _passwordResetTokenService.DeleteToken(passwordResetToken);
            
            

            return Ok("Password reset successfully.");
        }
    }

}
