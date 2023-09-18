using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySqlX.XDevAPI.Common;
using NaturalFirstAPI.Model;
using NaturalFirstAPI.Repository;
using NaturalFirstAPI.ViewModels;
//using NaturalFirstWebApp.ViewModels;

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
            if (user == null)
            {
                return BadRequest("Invalid user data");
            }
            try
            {
                var _user = _userRepository.GetUserLogin(user);
                if(user.Password == EncryptDecrypt.Decrypt(_user.Password))
                {
                    return Ok(_user);
                }
                else
                {
                    return Unauthorized();
                }
            }
            catch(Exception ex) {
                return StatusCode(500, "An error occurred while performing operation.");
            }
        }

        [HttpPost]
        public IActionResult RegisterUser(User user)
        {
            //https://localhost:7198/Home/SignUp?ejai2
            if (user == null)
            {
                return BadRequest("Invalid user data.");
            }
            try
            {
                var result = _userRepository.AddNewUser(user);
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "An error occurred while performing operation.");
            }
        }

        [HttpPost]
        public IActionResult ResetPassword(ResetPassword reset)
        {

            if (reset == null)
            {
                return BadRequest("Invalid user data.");
            }
            try
            {
                var result = _userRepository.ResetPassword(reset);
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "An error occurred while performing operation.");
            }
        }

        [HttpGet]
        public string ReturnString()
        {
            return "Output";
        }
    }
}
