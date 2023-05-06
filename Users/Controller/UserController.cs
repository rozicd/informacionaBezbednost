﻿using IB_projekat.Users.Model;
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

        public UserController(IUserService userService, IActivationTokenService activationTokenService, ISmsVerificationService smsVerificationService, IPasswordResetTokenService passwordResetTokenService)
        {
            _userService = userService;
            _activationTokenService = activationTokenService;
            _smsVerificationService = smsVerificationService;
            _passwordResetTokenService = passwordResetTokenService;

        }

        [HttpPost("register")]
        public async Task<IActionResult> AddUser(DTOS.CreateUserDTO user)
        {

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
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userService.Authenticate(model.Username, model.Password);
            if (user == null)
            {
                return Unauthorized();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email.ToString()),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            return Ok(user);
        }


        [HttpGet("authorized")]
        [Microsoft.AspNetCore.Authorization.Authorize(Policy = "AuthorizedOnly")]
        public IActionResult GetAuthorizedData()
        {
            // Only users with the Authorized or Admin role can access this action
            var userEmail = User.FindFirstValue(ClaimTypes.Name);
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



        [HttpPost("activateSms/{code}")]
        public async Task<IActionResult> ActivateUserSms(string code)
        {
            SmsVerificationCode smsVerificationCode = _smsVerificationService.GetCodeByCodeValue(code);
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
                if (smsCode.Expires >= DateTime.Now && _smsVerificationService.VerifyCode(code, smsCode.Code))
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
            // Validate the input
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            // Check if the email address is associated with a user
            var user = await _userService.GetByEmail(forgotPassword.Email);
            if (user == null)
            {
                return NotFound();
            }

            // Generate a password reset token and store it in the user record
            var token = await _passwordResetTokenService.GenerateToken(user.Id);

            // Send the password reset link to the user's email
            await _userService.SendPasswordResetEmail(user, token);

            return Ok();
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


        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDTO model)
        {
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

            await _passwordResetTokenService.DeleteToken(passwordResetToken);
            await _userService.ResetUserPassword(user.Id,user,model.NewPassword);

            return Ok("Password reset successfully.");
        }
    }

}
