using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NaturalFirstWebApp.Models;

namespace NaturalFirstWebApp.Controllers
{
    //[Authorize(Roles = "Admin")]
    [AuthorizationFilter(Roles = "Admin")]
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
