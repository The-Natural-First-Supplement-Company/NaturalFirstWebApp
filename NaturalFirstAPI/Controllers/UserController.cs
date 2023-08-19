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
    public class UserController : Controller
    {
        private readonly UserRepository _userRepository;
        public UserController(IConfiguration configuration)
        {
            _userRepository = new UserRepository(configuration);
        }

        [HttpPost]
        public IActionResult InsertBank(BankDetail bank)
        {
            var result = _userRepository.InsertBankAccount(bank);
            return Ok(result);
        }

        [HttpPost]
        public IActionResult GetUserInfo(User user)
        {
            var result = _userRepository.GetUserDetails(user);
            return Ok(result);
        }

        [HttpPost]
        public IActionResult UpdateUserInfo(User user)
        {
            var result = _userRepository.UpdateUserInfo(user);
            return Ok(result);
        }

        [HttpPost]
        public IActionResult ResetPaymentPassword(ResetPassword reset)
        {
            var result = _userRepository.ResetPaymentPassword(reset);
            return Ok(result);
        }
    }
}
