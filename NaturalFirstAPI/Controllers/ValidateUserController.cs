using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NaturalFirstAPI.Model;

namespace NaturalFirstAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValidateUserController : ControllerBase
    {
        public void/*JsonResult*/ RegisterUser(User user)
        {
            // return Json("");
        }
    }
}
