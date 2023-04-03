using IB_projekat.Users.Model;
using IB_projekat.Users.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IB_projekat.Users.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository<User> _userRepository;

        public UserController(IUserRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpPost]
        public IActionResult AddUser(User user)
        {
            _userRepository.Add(user);
            return Ok();
        }

        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, User user)
        {
            var existingUser = _userRepository.GetById(id);
            if (existingUser == null)
            {
                return NotFound();
            }
            existingUser.Email = user.Email;
            existingUser.Password = user.Password;
            _userRepository.Update(existingUser);
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            var existingUser = _userRepository.GetById(id);
            if (existingUser == null)
            {
                return NotFound();
            }
            _userRepository.Delete(existingUser);
            return Ok();
        }

        [HttpGet("{username}/{password}")]
        public IActionResult Authenticate(string username, string password)
        {
            var user = _userRepository.GetByEmailAndPassword(username, password);
            if (user == null)
            {
                return Unauthorized();
            }
            return Ok(user);
        }
    }
}
