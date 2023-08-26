using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NaturalFirstAPI.Model;
using NaturalFirstAPI.Repository;
using NaturalFirstWebApp.Models;
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

        [HttpPost]
        public IActionResult GetWalletBalance(User user)
        {
            if (user == null)
            {
                return BadRequest("Invalid user data.");
            }
            try
            {
                Decimal result = _userRepository.GetWalletBalance(user);
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "An error occurred while fetching wallet balance.");
            }
        }

        [HttpPost]
        public IActionResult RechargeWallet(RechargeVM recharge)
        {
            if (recharge == null)
            {
                return BadRequest("Invalid user data.");
            }
            try
            {
                var result = _userRepository.RechargeWallet(recharge);
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "An error occurred while fetching wallet balance.");
            }
        }

        [HttpPost]
        public IActionResult GetRechargeHistory(RechargeVM recharge)
        {
            if (recharge == null)
            {
                return BadRequest("Invalid user data.");
            }
            try
            {
                var result = _userRepository.GetRecharges(recharge);
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "An error occurred while fetching wallet balance.");
            }
        }

        [HttpPost]
        public IActionResult BankExistsInDB(User user)
        {
            if (user == null)
            {
                return BadRequest("Invalid user data.");
            }
            try
            {
                var result = _userRepository.IfBankExists(user);
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "An error occurred while fetching wallet balance.");
            }
        }

        [HttpPost]
        public IActionResult GetBankDetails(User user)
        {
            if (user == null)
            {
                return BadRequest("Invalid user data.");
            }
            try
            {
                var result = _userRepository.GetBankDetail(user);
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "An error occurred while fetching wallet balance.");
            }
        }
        // Pending Stored Procedure
        [HttpPost]
        public IActionResult GetActualBalance(User user)
        {
            if (user == null)
            {
                return BadRequest("Invalid user data.");
            }
            try
            {
                var result = _userRepository.GetBankDetail(user);
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "An error occurred while fetching wallet balance.");
            }
        }

        [HttpPost]
        public IActionResult AddWithdrawRequest(WithdrawVM withdraw)
        {
            if (withdraw == null)
            {
                return BadRequest("Invalid user data.");
            }
            try
            {
                var result = _userRepository.InsertWithdrawRequest(withdraw);
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "An error occurred while fetching wallet balance.");
            }
        }
    }
}
