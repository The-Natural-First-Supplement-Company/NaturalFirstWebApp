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

namespace NaturalFirstWebApp.Controllers
{
    public class CommonController : Controller
    {
        //private readonly IConfiguration _configuration;

        //public CommonController(IConfiguration configuration)
        //{
        //    _configuration = configuration;
        //}

        //public IActionResult GetImage(byte[] imageBytes, int height = 120, int width = 120)
        //{
        //    if(imageBytes.Length > 0)
        //    {
        //        // Convert bytes to an ImageSharp image
        //        using (MemoryStream ms = new MemoryStream(imageBytes))
        //        {
        //            using (Image image = Image.Load(ms))
        //            {
        //                int maxWidth = width;
        //                int maxHeight = height;
        //                image.Mutate(x => x.Resize(new ResizeOptions
        //                {
        //                    Size = new Size(maxWidth, maxHeight),
        //                    Mode = ResizeMode.Max
        //                }));

        //                // Convert ImageSharp image to byte array
        //                using (MemoryStream outputMs = new MemoryStream())
        //                {
        //                    image.Save(outputMs, new JpegEncoder()); // Change encoder if needed
        //                    return File(outputMs.ToArray(), "image/jpeg"); // Change content type if needed
        //                }
        //            }
        //        }
        //    }
        //    else
        //    {
        //        return NotFound();
        //    }
        //}

        [AuthorizationFilter(Roles ="User,Admin")]
        public string CurrentUser()
        {
            // Retrieve the user's claims
            List<Claim> userClaims = User.Claims.ToList();

            // Retrieve specific claim values
            var email = userClaims.FirstOrDefault(claim => claim.Type == ClaimTypes.Name)?.Value;

            return email;
        }
    }
}
