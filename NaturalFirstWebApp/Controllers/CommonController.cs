using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;
using System.Security.Claims;
using NaturalFirstWebApp.ViewModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using NaturalFirstWebApp.Models;
using Newtonsoft.Json;

namespace NaturalFirstWebApp.Controllers
{
    public class CommonController : Controller
    {
        [AuthorizationFilter(Roles ="User,Admin")]
        public string CurrentUser()
        {
            // Retrieve the user's claims
            List<Claim> userClaims = User.Claims.ToList();

            // Retrieve specific claim values
            var email = userClaims.FirstOrDefault(claim => claim.Type == ClaimTypes.Name)?.Value;

            return email;
        }

        [AuthorizationFilter(Roles = "User,Admin")]
        public int GetCurrentUserId()
        {
            // Retrieve the user's claims
            List<Claim> userClaims = User.Claims.ToList();

            // Retrieve specific claim values
            var Id = userClaims.FirstOrDefault(claim => claim.Type == "UserId")?.Value;

            return Convert.ToInt32(Id);
        }
    }
}
