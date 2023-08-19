using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NaturalFirstWebApp.Models;
using System.Data;
using System.Security.Claims;
using NaturalFirstWebApp.ViewModels;
using Newtonsoft.Json;
using System.Text;
using Org.BouncyCastle.Asn1.Ocsp;
using Microsoft.AspNetCore.Html;

namespace NaturalFirstWebApp.Controllers
{
    [AuthorizationFilter(Roles = "User")]
    public class UserController : CommonController
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public UserController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public IActionResult Index()
        {
            // Retrieve the user's claims
            var userClaims = User.Claims.ToList();

            // Retrieve specific claim values
            ViewBag.Referral = userClaims.FirstOrDefault(c => c.Type == "Referral")?.Value;

            return View();
        }

        public IActionResult Equipment()
        {
            return View();
        }

        public IActionResult Recharge()
        {
            return View();
        }

        public IActionResult Withdraws()
        {
            return View();
        }
        
        public IActionResult Salary()
        {
            return View();
        }
        
        //Me Options
        public IActionResult Profile()
        {
            //User responseData = new User();
            //try
            //{
            //    // Create an instance of HttpClient using the named client from the factory
            //    var client = _httpClientFactory.CreateClient("MyApiClient");

            //    //client.DefaultRequestHeaders.Add("Token","");

            //    // Define the endpoint path
            //    var endpointPath = "/api/User/GetUserInfo"; // Replace with the actual login endpoint path

            //    // Prepare the content with parameters
            //    var requestData = new
            //    {
            //        Email = CurrentUser()
            //    };
            //    var json = JsonConvert.SerializeObject(requestData);
            //    var content = new StringContent(json, Encoding.UTF8, "application/json");

            //    // Make a POST request to the API
            //    var response = await client.PostAsync(endpointPath, content);
            //    var jsonResponse = await response.Content.ReadAsStringAsync();
            //    // Deserialize the JSON response into an object
            //    responseData = JsonConvert.DeserializeObject<User>(jsonResponse);

            //}
            //catch (Exception ex)
            //{
            //    ViewBag.Error = "Unable to fetch information. Please try again later.";
            //}
            return View();
        }

        public IActionResult AddBank()
        {
            if(TempData["Message"] != null)
            {
                ViewBag.Message = TempData["Message"];
                TempData.Remove("Message");
            }
            if(TempData["ClearForm"] != null)
            {
                ViewBag.ClearForm = TempData["ClearForm"];
                TempData.Remove("ClearForm");
            }
            if (TempData["Error"] != null)
            {
                ViewBag.Error = TempData["Error"].ToString();
                TempData.Remove("Error");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddBank(BankVM bank)
        {
            TempData["Error"] = null;
            if (ModelState.IsValid)
            {
                Common responseData = new Common();
                try
                {
                    // Create an instance of HttpClient using the named client from the factory
                    var client = _httpClientFactory.CreateClient("MyApiClient");

                    //client.DefaultRequestHeaders.Add("Token","");

                    // Define the endpoint path
                    var endpointPath = "/api/User/InsertBank"; // Replace with the actual login endpoint path

                    // Prepare the content with parameters
                    var requestData = new
                    {
                        BankName = bank.BankName,
                        AccountNo = BCrypt.Net.BCrypt.HashPassword(bank.AccountNo),
                        IFSCCode = BCrypt.Net.BCrypt.HashPassword(bank.IFSCCode),
                        RealName = bank.RealName,
                        TrnPassword = BCrypt.Net.BCrypt.HashPassword(bank.TrnPassword),
                        email = CurrentUser()
                    };
                    var json = JsonConvert.SerializeObject(requestData);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    // Make a POST request to the API
                    var response = await client.PostAsync(endpointPath, content);
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    // Deserialize the JSON response into an object
                    responseData = JsonConvert.DeserializeObject<Common>(jsonResponse);
                }
                catch (Exception ex)
                {
                    responseData.StatusId = 0;
                    responseData.Status = ex.Message;
                }
                if (responseData.StatusId == 1)
                {
                    TempData["Message"] = responseData.Status;
                    TempData["ClearForm"] = true;
                }
                else
                {
                    TempData["Message"] = responseData.Status;
                }
            }
            else
            {
                TempData["Error"] = "Please fill all the details";
            }
            return RedirectToAction("AddBank","User");
        }

        [Route("{controller}/Me/Settings")]
        public IActionResult Settings()
        {
            return View();
        }

        //[Route("{controller}/Me/Info")]
        public async Task<IActionResult> Information()
        {
            User responseData = new User();
            try
            {
                // Create an instance of HttpClient using the named client from the factory
                var client = _httpClientFactory.CreateClient("MyApiClient");

                //client.DefaultRequestHeaders.Add("Token","");

                // Define the endpoint path
                var endpointPath = "/api/User/GetUserInfo"; // Replace with the actual login endpoint path

                // Prepare the content with parameters
                var requestData = new
                {
                    Email = CurrentUser()
                };
                var json = JsonConvert.SerializeObject(requestData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Make a POST request to the API
                var response = await client.PostAsync(endpointPath, content);
                var jsonResponse = await response.Content.ReadAsStringAsync();
                // Deserialize the JSON response into an object
                responseData = JsonConvert.DeserializeObject<User>(jsonResponse);

            }
            catch (Exception ex)
            {
                ViewBag.Error = "Unable to fetch information. Please try again later.";
            }
            return View(responseData);
        }

        //[Route("{controller}/Me/InfoPost")]
        [HttpPost]
        public async Task<IActionResult> Information(User info, IFormFile imageFile)
        {
            Common responseData = new Common();
            try
            {
                if (imageFile != null && imageFile.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        imageFile.CopyTo(memoryStream);
                        info.ProfilePic = memoryStream.ToArray();
                    }
                }
                // Create an instance of HttpClient using the named client from the factory
                var client = _httpClientFactory.CreateClient("MyApiClient");

                //client.DefaultRequestHeaders.Add("Token","");

                // Define the endpoint path
                var endpointPath = "/api/User/UpdateUserInfo"; // Replace with the actual login endpoint path

                // Prepare the content with parameters
                var requestData = new
                {
                    Email = CurrentUser(),
                    NickName = info.NickName,
                    ProfilePic = info.ProfilePic
                };
                var json = JsonConvert.SerializeObject(requestData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Make a POST request to the API
                var response = await client.PostAsync(endpointPath, content);
                var jsonResponse = await response.Content.ReadAsStringAsync();
                // Deserialize the JSON response into an object
                responseData = JsonConvert.DeserializeObject<Common>(jsonResponse);

            }
            catch (Exception ex)
            {
                ViewBag.Error = "Unable to fetch information. Please try again later.";
            }
            return RedirectToAction("Information");
        }

        //Reset Password
        //[Route("{controller}/Me/ResetLoginPassword")]
        public IActionResult PasswordReset()
        {
            ViewBag.Email = CurrentUser();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> PasswordReset(ResetPassword reset)
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

                    string password = BCrypt.Net.BCrypt.HashPassword(reset.Password);

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
            return RedirectToAction("Index","User");
        }


        //Reset Payment Password
        //[Route("{controller}/Me/ResetPaymentPassword")]
        public IActionResult PaymentReset()
        {
            ViewBag.Email = CurrentUser();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PaymentReset(ResetPassword reset)
        {
            ResetPassword rp = new ResetPassword();
            if (ModelState.IsValid && reset.VerificationCode == TempData["VerificationCode"].ToString())
            {
                try
                {
                    // Create an instance of HttpClient using the named client from the factory
                    var client = _httpClientFactory.CreateClient("MyApiClient");

                    // Define the endpoint path
                    var endpointPath = "/api/User/ResetPaymentPassword"; // Replace with the actual login endpoint path

                    string password = BCrypt.Net.BCrypt.HashPassword(reset.Password);

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
            return RedirectToAction("Index", "User");
        }
    }
}
