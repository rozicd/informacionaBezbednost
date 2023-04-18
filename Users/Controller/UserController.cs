using IB_projekat.Users.Model;
using IB_projekat.Users.Repository;
using IB_projekat.Users.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using IB_projekat.Users.DTOS;

namespace IB_projekat.Users.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(DTOS.CreateUserDTO user)
        {
            await _userService.AddUser(user);
            return Ok();
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
    }
}
