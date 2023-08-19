using BCrypt.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NaturalFirstAPI.Model;
using NaturalFirstAPI.Repository;
using NaturalFirstWebApp.ViewModels;

namespace NaturalFirstAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly UserRepository _userRepository;
        public LoginController(IConfiguration configuration)
        {
            _userRepository = new UserRepository(configuration);
        }


        [HttpPost]
        public IActionResult VerifyUser(User user)
        {
            var _user = _userRepository.GetUserLogin(user);
            if (_user == null)
            {
                return NotFound();
            }
            else if (BCrypt.Net.BCrypt.Verify(user.Password, _user.Password))
            {
                return Ok(_user);
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpPost]
        public IActionResult RegisterUser(User user)
        {
            var _common = _userRepository.AddNewUser(user);
            if (_common.StatusId == 1)
            {
                return Ok(_common);
            }
            else
            {
                return BadRequest(_common);
            }
        }

        [HttpPost]
        public IActionResult ResetPassword(ResetPassword reset)
        {
            var _common = _userRepository.ResetPassword(reset);
            if (_common.StatusId == 1)
            {
                return Ok(_common);
            }
            else
            {
                return BadRequest(_common);
            }
        }

        [HttpGet]
        public string ReturnString()
        {
            return "Output";
        }
    }
}
