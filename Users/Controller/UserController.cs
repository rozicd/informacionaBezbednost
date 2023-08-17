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
                _logger.Information("New user registered - Email: {Email}", user.Email);

                return Ok();
            }
            else
            {
                _logger.Information("Registration failed - User with the same email already exists - Email: {Email}", user.Email);
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
                _logger.Information("User update failed - User not found - UserId: {UserId}", id);

                return NotFound();
            }
            _logger.Information("User updated - UserId: {UserId}", id);

            return Ok();
        }

        [Microsoft.AspNetCore.Authorization.AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO model)
        {
            _logger.Information("User login attempt - Email: {Email}", model.Username);

            if (! await _recaptchaVerifier.VerifyRecaptcha(model.RecaptchaToken))
            {
                _logger.Information("Recaptcha verification failed for user login - Email: {Email}", model.Username);

                return BadRequest("Recaptcha is not valid!");
            }

            if (!ModelState.IsValid)
            {
                _logger.Warning("Login Failed Because of invalid ModelState: {Email}", model.Username);

                return BadRequest(ModelState);
            }
            
            var passwordStatus = await _userService.Authenticate(model.Username, model.Password);
            if (passwordStatus == PasswordStatus.INACTIVE)
            {
                _logger.Warning("Login failed because password is invalid: {Email}", model.Username);

                return Unauthorized();
            }
            User user = await _userService.GetByEmail(model.Username);
            if (passwordStatus == PasswordStatus.EXPIRED)
            {
                PasswordResetToken token = await _passwordResetTokenService.GenerateToken(user.Id);
                string redirectUrl = "http://localhost:3000/reset-password?id=" + user.Id + "&token=" + token.Token;
                _logger.Information("Login successful but needs password change: {Email}", model.Username);

                return Ok(new { redirectUrl });
                
            }


            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email.ToString()),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
                new Claim("TwoFactorVerified", "false")
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            if (user.Role == UserType.Unauthorized) {
                _logger.Warning("Login failed email not verified: {Email}", model.Username);
            return Unauthorized("Email not verified");
            }
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity));
            _logger.Information("Login successful: {Email}", model.Username);
            
            return Ok(user);
        }




        [Microsoft.AspNetCore.Authorization.AllowAnonymous]
        [HttpGet("/api/user/google-login")]
        public IActionResult GoogleLogin()
        {
            _logger.Information("User OAuth login attempt");

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
                    _logger.Information("OAuth login successful!: {Email}", email);

                    return Redirect("http://localhost:3000/home");
                }
                else
                {
                    _logger.Warning("OAuth Login failed, because email is already registered with different method!: {Email}", email);

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
                _logger.Information("OAuth registration successful!: {Email}", email);

                return Redirect("http://localhost:3000/home");
            }
           

        }
        [HttpGet("authorized")]
        [Microsoft.AspNetCore.Authorization.Authorize(Policy = "AuthorizedOnly")]
        public IActionResult GetAuthorizedData()
        {

            var userEmail = User.FindFirstValue(ClaimTypes.Name);
            _logger.Information("Authorized data request - UserEmail: {UserEmail}", User.FindFirstValue(ClaimTypes.Name));

            if (User.FindFirstValue("TwoFactorVerified") == "false")
            {
                _logger.Information("Access denied - Two-factor authentication not verified - UserEmail: {UserEmail}", User.FindFirstValue(ClaimTypes.Name));

                return Forbid();
            }
            _logger.Information("Access granted - Authorized data retrieved - UserEmail: {UserEmail}", User.FindFirstValue(ClaimTypes.Name));

            return Ok(userEmail);
        }


        [HttpPut("activate/{id}/{token}")]
        public async Task<IActionResult> ActivateUserAsync(int id, string token)
        {
            _logger.Information("Activating user - UserId: {UserId}, Token: {Token}", id, token);

            User user = await _userService.GetById(id);
            if (user == null)
            {
                _logger.Warning("User not found - UserId: {UserId}", id);
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
                _logger.Warning("Invalid or expired activation token - UserId: {UserId}", id);

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

            _logger.Information("User activated successfully - UserId: {UserId}", id);

            return Ok();
        }

        [HttpPost("2fa/activate/{code}")]
        public async Task<IActionResult> Activate2fa(string code)
        {
            _logger.Information("Activating 2FA - Code: {Code}", code);

            SmsVerificationCode smsVerificationCode = _smsVerificationService.GetCodeByCodeValueAndType(code, VerificationType.TWO_FACTOR);
            if (smsVerificationCode == null)
            {
                _logger.Warning("SMS verification code not found - Code: {Code}", code);
                return NotFound();
            }

            User user = await _userService.GetById(smsVerificationCode.UserId);
            if (user == null)
            {
                await _smsVerificationService.DeleteCode(smsVerificationCode);
                _logger.Warning("User not found - UserId: {UserId}", smsVerificationCode.UserId);
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
                _logger.Warning("Invalid or expired SMS activation token - Code: {Code}", code);

                foreach (var smsCode in smsCodes)
                {
                    await _smsVerificationService.DeleteCode(smsCode);
                }

                return BadRequest("Invalid or expired SMS activation token");
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

            _logger.Information("2FA activated successfully - Code: {Code}", code);

            return Ok();
        }





        [HttpPost("activateSms/{code}")]
        public async Task<IActionResult> ActivateUserSms(string code)
        {
            _logger.Information("Activating user via SMS - Code: {Code}", code);

            SmsVerificationCode smsVerificationCode = _smsVerificationService.GetCodeByCodeValueAndType(code, VerificationType.VERIFICATION);
            if (smsVerificationCode == null)
            {
                _logger.Warning("SMS verification code not found - Code: {Code}", code);
                return NotFound();
            }

            User user = await _userService.GetById(smsVerificationCode.UserId);
            if (user == null)
            {
                await _smsVerificationService.DeleteCode(smsVerificationCode);
                _logger.Warning("User not found - UserId: {UserId}", smsVerificationCode.UserId);
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
                _logger.Warning("Invalid or expired SMS activation token - Code: {Code}", code);

                foreach (var smsCode in smsCodes)
                {
                    await _smsVerificationService.DeleteCode(smsCode);
                }

                await _userService.DeleteUser(user.Id);
                return BadRequest("Invalid or expired SMS activation token");
            }

            // Activate the user account
            user.Role = UserType.Authorized;
            await _userService.UpdateUser(user.Id, user);

            // Delete the activation token(s)
            foreach (var smsCode in smsCodes)
            {
                await _smsVerificationService.DeleteCode(smsCode);
            }

            _logger.Information("User activated via SMS successfully - Email: {UserId}", user.Email);

            return Ok();
        }

        [HttpPost("forgotpassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDTO forgotPassword)
        {
            _logger.Information("Forgot password request - Email: {Email}", forgotPassword.Email);

            if (!await _recaptchaVerifier.VerifyRecaptcha(forgotPassword.RecaptchaToken))
            {
                _logger.Warning("Recaptcha is not valid for forgot password request - Email: {Email}", forgotPassword.Email);
                return BadRequest("Recaptcha is not valid!");
            }

            if (!ModelState.IsValid)
            {
                _logger.Warning("Invalid model state for forgot password request - Email: {Email}", forgotPassword.Email);
                return BadRequest();
            }

            var user = await _userService.GetByEmail(forgotPassword.Email);
            if (user == null)
            {
                _logger.Warning("User not found for forgot password request - Email: {Email}", forgotPassword.Email);
                return NotFound();
            }

            if (!user.IsOAuth)
            {
                var token = await _passwordResetTokenService.GenerateToken(user.Id);

                await _userService.SendPasswordResetEmail(user, token);

                _logger.Information("Password reset email sent successfully - Email: {Email}", forgotPassword.Email);

                return Ok();
            }
            else
            {
                _logger.Warning("User is already registered with a different method - Email: {Email}", forgotPassword.Email);
                return BadRequest("User is already registered with a different method!");
            }
        }


        [HttpPost("logout")]
        [Microsoft.AspNetCore.Authorization.Authorize(Policy = "AuthorizedOnly")]
        public async Task<IActionResult> Logout()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Name);
            _logger.Information("Logout request - User: {UserEmail}", userEmail);

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);


            _logger.Information("User logged out successfully - User: {UserEmail}", userEmail);

            return Ok();
        }

        [HttpPost("verify-password-reset-token/{token}")]
        public async Task<IActionResult> VerifyPasswordResetToken(string token)
        {
            _logger.Information("Password reset token verification request - Token: {Token}", token);

            var passwordResetToken = await _passwordResetTokenService.GetTokenByToken(token);

            if (passwordResetToken == null)
            {
                _logger.Warning("Invalid password reset token - Token: {Token}", token);
                return NotFound();
            }

            if (passwordResetToken.ExpirationDate < DateTime.Now)
            {
                await _passwordResetTokenService.DeleteToken(passwordResetToken);

                _logger.Warning("Expired password reset token - Token: {Token}", token);
                return BadRequest("The password reset token has expired.");
            }

            _logger.Information("Valid password reset token - Token: {Token}", token);
            return Ok("The password reset token is valid.");
        }


        [HttpPost("email")]
        public async Task<User> GetByEmail([FromBody] UserEmailDTO userEmailDTO)
        {
            _logger.Information("Get user by email request - Email: {Email}", userEmailDTO.email);

            User user = await _userService.GetByEmail(userEmailDTO.email);

            if (user != null)
            {
                _logger.Information("User found - Email: {Email}", userEmailDTO.email);
            }
            else
            {
                _logger.Warning("User not found - Email: {Email}", userEmailDTO.email);
            }

            return user;
        }

        [HttpPost("2fa/{type}")]
        public async Task<IActionResult> TwoFactorAuthentication(string type)
        {
            string email = User.FindFirstValue(ClaimTypes.Name);

            _logger.Information("Two-factor authentication request - Email: {Email}, Type: {Type}", email, type);

            bool twofa = false;

            if (type == "email")
            {
                twofa = await _userService.TwoFactorAuthentication(email);
            }
            else
            {
                twofa = await _userService.TwoFactorAuthenticationSMS(email);
            }

            _logger.Information("Two-factor authentication completed - Email: {Email}, Type: {Type}, Result: {Result}", email, type, twofa);

            return Ok();
        }



        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDTO model)
        {
            _logger.Information("Reset password request - ID: {Id}", model.Id);

            if (!await _recaptchaVerifier.VerifyRecaptcha(model.RecaptchaToken))
            {
                _logger.Warning("Recaptcha is not valid - ID: {Id}", model.Id);
                return BadRequest("Recaptcha is not valid!");
            }

            if (!ModelState.IsValid)
            {
                _logger.Warning("Invalid model state - ID: {Id}", model.Id);
                return BadRequest(ModelState);
            }

            var user = await _userService.GetById(model.Id);
            if (user == null)
            {
                _logger.Warning("Invalid password reset - User not found - ID: {Id}", model.Id);
                return NotFound("Password reset is invalid");
            }

            var passwordResetToken = await _passwordResetTokenService.GetTokenByToken(model.Token);

            if (passwordResetToken == null)
            {
                _logger.Warning("Invalid password reset - Token not found - ID: {Id}", model.Id);
                return NotFound();
            }

            if (passwordResetToken.ExpirationDate < DateTime.Now)
            {
                _logger.Warning("Invalid password reset - Token expired - ID: {Id}", model.Id);
                await _passwordResetTokenService.DeleteToken(passwordResetToken);
                return BadRequest("The password reset token has expired.");
            }

            bool success = await _userService.ResetUserPassword(user.Id, user, model.NewPassword);
            if (!success)
            {
                _logger.Warning("Failed to reset password - ID: {Id}", model.Id);
                return BadRequest("You cannot use the same password!");
            }

            await _passwordResetTokenService.DeleteToken(passwordResetToken);

            _logger.Information("Password reset successful - ID: {Id}", model.Id);
            return Ok("Password reset successfully.");
        }

    }

}
