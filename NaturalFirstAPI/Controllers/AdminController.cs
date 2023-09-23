using Microsoft.AspNetCore.Mvc;
using NaturalFirstAPI.Model;
using NaturalFirstAPI.Models;
using NaturalFirstAPI.Repository;
using NaturalFirstAPI.ViewModels;

namespace NaturalFirstAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly AdminRepository _adminRepository;
        public AdminController(IConfiguration configuration)
        {
            _adminRepository = new AdminRepository(configuration);
        }
        //List of Pending Recharge
        [HttpPost]
        public IActionResult GetPendingRecharge(User user)
        {
            try
            {
                var result = _adminRepository.GetPendingRechargeList(user.Email);
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "An error occurred while fetching all products.");
            }
        }
        //Pending Recharge by Id
        [HttpPost]
        public IActionResult GetPendingRechargeDetail(RechargeHistoryVM rc)
        {
            try
            {
                var result = _adminRepository.GetRechargeDetail(rc);
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "An error occurred while fetching all products.");
            }
        }

        [HttpPost]
        public IActionResult UpdateRechargeById(RechargeHistoryVM recharge)
        {
            try
            {
                var result = _adminRepository.UpdateRechargeStatus(recharge);
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "An error occurred while fetching all products.");
            }
        }

        [HttpPost]
        public IActionResult GetPendingWithdraw(User user)
        {
            try
            {
                var result = _adminRepository.GetPendingWithdrawList(user.Email);
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "An error occurred while fetching all products.");
            }
        }

        [HttpPost]
        public IActionResult GetPendingWithdrawDetail(WithdrawVM withdraw)
        {
            try
            {
                var result = _adminRepository.GetWithdrawDetailById(withdraw);
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "An error occurred while fetching all products.");
            }
        }

        [HttpPost]
        public IActionResult UpdateWithdrawById(WithdrawVM vm)
        {
            try
            {
                var result = _adminRepository.UpdateWithdrawStatus(vm);
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "An error occurred while fetching all products.");
            }
        }

        [HttpPost]
        public IActionResult GetRechargeHistory(User user)
        {
            try
            {
                var result = _adminRepository.GetRechargeList(user.Email);
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "An error occurred while fetching all products.");
            }
        }

        [HttpPost]
        public IActionResult GetWithdrawHistory(User user)
        {
            try
            {
                var result = _adminRepository.GetWithdrawList(user.Email);
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "An error occurred while fetching all products.");
            }
        }

        [HttpGet]
        public IActionResult UpdateDaily()
        {
            try
            {
                var result = _adminRepository.UpdateDailyIncome();
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "An error occurred while fetching all products.");
            }
        }
    }
}
