using Microsoft.AspNetCore.Mvc;
using NaturalFirstAPI.Model;
using NaturalFirstAPI.Models;
using NaturalFirstAPI.Repository;
using NaturalFirstAPI.ViewModels;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NaturalFirstAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ProductRepository _prdRepository;
        public ProductController(IConfiguration configuration)
        {
            _prdRepository = new ProductRepository(configuration);
        }

        [HttpGet]
        public IActionResult GetAllProducts()
        {
            try
            {
                var result = _prdRepository.GetAllProducts();
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "An error occurred while fetching all products.");
            }
        }

        [HttpGet] // Change from [HttpPost] to [HttpGet]
        public IActionResult GetProductById(int IdProducts)
        {
            if (IdProducts < 1)
            {
                return BadRequest("Invalid user data.");
            }
            try
            {
                var result = _prdRepository.GetProduct(IdProducts);
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "An error occurred while fetching product by id.");
            }
        }


        [HttpPost]
        public IActionResult PurchaseProduct(PurchaseVM prd)
        {
            if (prd == null)
            {
                return BadRequest("Invalid user data.");
            }
            try
            {
                var result = _prdRepository.PurchaseProduct(prd);
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "An error occurred while purchasing product.");
            }
        }

        [HttpPost]
        public IActionResult InsertProduct(ProductVM prd)
        {
            if (prd == null)
            {
                return BadRequest("Invalid user data.");
            }
            try
            {
                var result = _prdRepository.AddNewProduct(prd);
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "An error occurred while purchasing product.");
            }
        }

        [HttpPost]
        public IActionResult GetMyTeamProducts(User user)
        {
            if (user == null)
            {
                return BadRequest("Invalid user data.");
            }
            try
            {
                var result = _prdRepository.GetTeamProduct(user);
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "An error occurred while purchasing product.");
            }
        }
    }
}
