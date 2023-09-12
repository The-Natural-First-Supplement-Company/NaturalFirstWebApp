using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NaturalFirstWebApp.Models;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using NaturalFirstWebApp.ViewModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Org.BouncyCastle.Asn1.Ocsp;

namespace NaturalFirstWebApp.Controllers
{
    public class HomeController : Controller
    {
        const string SessionUser = "_Email";
        const string SessionRole = "_Role";
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public HomeController(IHttpClientFactory httpClientFactory, ILogger<HomeController> logger)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(string email, string password)
        {
            if (email != null && password != null)
            {
                try
                {
                    // Create an instance of HttpClient using the named client from the factory
                    var client = _httpClientFactory.CreateClient("MyApiClient");

                    // Define the endpoint path
                    var endpointPath = "/api/Login/VerifyUser"; // Replace with the actual login endpoint path

                    // Prepare the content with parameters
                    var requestData = new { Email = email, Password = password };
                    var json = JsonConvert.SerializeObject(requestData);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    // Make a POST request to the API
                    var response = await client.PostAsync(endpointPath, content);

                    if (response.IsSuccessStatusCode)
                    {
                        var jsonResponse = await response.Content.ReadAsStringAsync();
                        // Deserialize the JSON response into an object
                        var responseData = JsonConvert.DeserializeObject<User>(jsonResponse);

                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, email),
                            new Claim(ClaimTypes.Role, responseData.Role),
                            new Claim("Referral", responseData.ReferralCode),
                            new Claim("UserId", responseData.Id.ToString())
                        };

                        // Create claims identity
                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                        // Create authentication properties
                        var authProperties = new AuthenticationProperties
                        {
                            IsPersistent = true, // You can change this based on your requirements
                        };

                        // Sign in the user
                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                            new ClaimsPrincipal(claimsIdentity), authProperties);

                        // Process the responseData object
                        if (responseData.Role == "User")
                        {
                            return RedirectToAction("Index", "User");
                        }
                        else
                        {
                            return RedirectToAction("Index", "Admin");
                        }
                    }
                    else
                    {
                        ViewBag.msg = "Login Failed!";
                        return View();
                    }
                }
                catch (Exception ex)
                {
                    // Handle exceptions
                    return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred: {ex.Message}");
                }
            }
            else
            {
                ViewBag.msg = "Email or Password is null!";
                return View();
            }
        }

        [HttpGet]
        public IActionResult SignUp(string referralcode)
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpVM signUp)
        {
            if (signUp.Email != null)
            {
                try
                {
                    // Create an instance of HttpClient using the named client from the factory
                    var client = _httpClientFactory.CreateClient("MyApiClient");

                    // Define the endpoint path
                    var endpointPath = "/api/Login/RegisterUser"; // Replace with the actual login endpoint path

                    string password = EncryptDecrypt.Encrypt(signUp.Password);

                    // Prepare the content with parameters
                    var requestData = new { Email = signUp.Email, Password = password, ReferralCode = signUp.ReferralCode };
                    var json = JsonConvert.SerializeObject(requestData);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    // Make a POST request to the API
                    var response = await client.PostAsync(endpointPath, content);
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    // Deserialize the JSON response into an object
                    var responseData = JsonConvert.DeserializeObject<Common>(jsonResponse);
                    if (response.IsSuccessStatusCode)
                    {
                        return Json(responseData);
                    }
                    else
                    {
                        return Json(responseData);
                    }
                }
                catch (Exception ex)
                {
                    // Handle exceptions
                    return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred: {ex.Message}");
                }
            }
            else 
            { 
                return BadRequest(); 
            }
        }

        public IActionResult ForgotPassword()
        {
            ResetPassword reset = new ResetPassword();
            if(TempData["Result"] != null)
            {
                Common common = JsonConvert.DeserializeObject<Common>(TempData["Result"].ToString());
                ViewBag.StatudId = common.StatusId;
                ViewBag.msg = common.Status;
                return View(reset);
            }
            else
            {
                return View(reset);
            }
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ResetPassword reset)
        {
            ResetPassword rp = new ResetPassword();
            if (ModelState.IsValid && reset.VerificationCode == TempData["VerificationCode"].ToString())
            {
                try
                {
                    // Create an instance of HttpClient using the named client from the factory
                    var client = _httpClientFactory.CreateClient("MyApiClient");

                    // Define the endpoint path
                    var endpointPath = "/api/Login/ResetPassword"; // Replace with the actual login endpoint path

                    string password = EncryptDecrypt.Encrypt(reset.Password);

                    // Prepare the content with parameters
                    var requestData = new { Email = reset.Email, Password = password };
                    var json = JsonConvert.SerializeObject(requestData);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    // Make a POST request to the API
                    var response = await client.PostAsync(endpointPath, content);
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    // Deserialize the JSON response into an object
                    var responseData = JsonConvert.DeserializeObject<Common>(jsonResponse);

                    if (response.IsSuccessStatusCode)
                    {
                        TempData["Result"] = jsonResponse;
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        TempData["Result"] = jsonResponse;
                        return View(rp);
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.StatudId = 0;
                    ViewBag.msg = ex.Message;
                    return View(rp);
                }
            }
            return View(rp);
        }

        //[ValidateAntiForgeryToken]
        [AuthorizationFilter(Roles ="Admin,User")]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Home"); // Redirect to a specific page after logout
        }

        [HttpGet]
        public string SendOTP(string email)
        {
            string code = Send_Email.SendEmailVerification(email);
            TempData["VerificationCode"] = code;
            return code;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}