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

namespace IB_projekat.Users.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IActivationTokenService _activationTokenService;

        public UserController(IUserService userService, IActivationTokenService activationTokenService)
        {
            _userService = userService;
            _activationTokenService = activationTokenService;
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
                return BadRequest("USER WITH THIS EMAIL ALREADY EXIST, IDIOT!!!!");
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
                new Claim(ClaimTypes.Name, user.Email),
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


        [HttpGet("admin")]
        [Microsoft.AspNetCore.Authorization.Authorize(Policy = "AdminOnly")]
        public IActionResult GetAdminData()
        {
            // Only users with the Admin role can access this action
            return Ok("Admin data");
        }

        [HttpGet("authorized")]
        [Microsoft.AspNetCore.Authorization.Authorize(Policy = "AuthorizedOnly")]
        public IActionResult GetAuthorizedData()
        {
            // Only users with the Authorized or Admin role can access this action
            return Ok("Authorized data");
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



    }
}
