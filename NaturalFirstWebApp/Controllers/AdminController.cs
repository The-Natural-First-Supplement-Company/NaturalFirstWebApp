using Google.Apis.Gmail.v1.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.FlowAnalysis.DataFlow;
using Microsoft.Extensions.Logging;
using NaturalFirstWebApp.Models;
using NaturalFirstWebApp.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Text;

namespace NaturalFirstWebApp.Controllers
{
    //[Authorize(Roles = "Admin")]
    [AuthorizationFilter(Roles = "Admin")]
    public class AdminController : CommonController
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public AdminController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AddProducts()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddProducts(ProductVM info, IFormFile imageFile)
        {
            Common responseData = new Common();
            try
            {
                if (imageFile != null && imageFile.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        imageFile.CopyTo(memoryStream);
                        info.ProductImage = memoryStream.ToArray();
                    }
                }
                // Create an instance of HttpClient using the named client from the factory
                var client = _httpClientFactory.CreateClient("MyApiClient");

                //client.DefaultRequestHeaders.Add("Token","");

                // Define the endpoint path
                var endpointPath = "/api/Product/InsertProduct"; // Replace with the actual login endpoint path
                
                // Prepare the content with parameters
                var requestData = new
                {
                    Email = CurrentUser(),
                    Cycle = info.Cycle,
                    Description = info.Description,
                    ProductName = info.ProductName,
                    IncomePerDay = info.IncomePerDay,
                    InvestAmt = info.InvestAmt,
                    ProductImage = info.ProductImage,
                    TotalAmt = info.TotalAmt
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
            return RedirectToAction("Products");
        }

        public async Task<IActionResult> Products()
        {
            try
            {
                // Create an instance of HttpClient using the named client from the factory
                var client = _httpClientFactory.CreateClient("MyApiClient");

                // Define the endpoint path
                var endpointPath = "/api/Product/GetAllProducts"; // Replace with the actual endpoint path

                // Make a GET request to the API
                var response = await client.GetAsync(endpointPath);
                var jsonResponse = await response.Content.ReadAsStringAsync();

                // Deserialize the JSON response into a List of Products
                var responseData = JsonConvert.DeserializeObject<List<Products>>(jsonResponse);

                return View(responseData);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View();
            }
        }

        public async Task<IActionResult> Recharge()
        {
            try
            {
                // Create an instance of HttpClient using the named client from the factory
                var client = _httpClientFactory.CreateClient("MyApiClient");

                // Define the endpoint path
                var endpointPath = "/api/Admin/GetPendingRecharge"; // Replace with the actual endpoint path

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
                
                // Deserialize the JSON response into a List of Products
                var responseData = JsonConvert.DeserializeObject<List<RechargeListVM>>(jsonResponse);

                return View(responseData);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View();
            }
        }

        public async Task<IActionResult> Withdraw()
        {
            try
            {
                // Create an instance of HttpClient using the named client from the factory
                var client = _httpClientFactory.CreateClient("MyApiClient");

                // Define the endpoint path
                var endpointPath = "/api/Admin/GetPendingWithdraw"; // Replace with the actual endpoint path

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

                // Deserialize the JSON response into a List of Products
                var responseData = JsonConvert.DeserializeObject<List<AdminWithdrawVM>>(jsonResponse);

                return View(responseData);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View();
            }
        }

        public IActionResult Options()
        {
            return View();
        }

        //Reset Password
        public IActionResult PasswordReset()
        {
            string Email = CurrentUser();
            ViewData["Email"] = Email;
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
            return RedirectToAction("Index", "Admin");
        }

        public async Task<IActionResult> RechargeHistory()
        {
            try
            {
                // Create an instance of HttpClient using the named client from the factory
                var client = _httpClientFactory.CreateClient("MyApiClient");

                // Define the endpoint path
                var endpointPath = "/api/Admin/GetRechargeHistory"; // Replace with the actual endpoint path

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

                // Deserialize the JSON response into a List of Products
                var responseData = JsonConvert.DeserializeObject<List<RechargeListVM>>(jsonResponse);

                return View(responseData);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View();
            }
        }

        public async Task<IActionResult> WithdrawHistory()
        {
            try
            {
                // Create an instance of HttpClient using the named client from the factory
                var client = _httpClientFactory.CreateClient("MyApiClient");

                // Define the endpoint path
                var endpointPath = "/api/Admin/GetWithdrawHistory"; // Replace with the actual endpoint path

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

                // Deserialize the JSON response into a List of Products
                var responseData = JsonConvert.DeserializeObject<List<AdminWithdrawVM>>(jsonResponse);

                return View(responseData);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View();
            }
        }

        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Home"); // Redirect to a specific page after logout
        }

        /// <summary>
        /// below methods are called by JQuery ajax methods;
        /// </summary>
        /// 
        ///
        [HttpGet]
        public async Task<JsonResult> GetRechargeById(int Id)
        {
            try
            {
                // Create an instance of HttpClient using the named client from the factory
                var client = _httpClientFactory.CreateClient("MyApiClient");

                // Define the endpoint path
                var endpointPath = "/api/Admin/GetPendingRechargeDetail"; // Replace with the actual endpoint path

                // Prepare the content with parameters
                var requestData = new
                {
                    Email = CurrentUser(),
                    IdHistory = Id
                };

                var json = JsonConvert.SerializeObject(requestData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Make a POST request to the API
                var response = await client.PostAsync(endpointPath, content);
                var jsonResponse = await response.Content.ReadAsStringAsync();

                // Deserialize the JSON response into a List of Products
                var responseData = JsonConvert.DeserializeObject<RechargeListVM>(jsonResponse);

                return Json(responseData);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return Json(null);
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetWithdrawById(int Id)
        {
            try
            {
                // Create an instance of HttpClient using the named client from the factory
                var client = _httpClientFactory.CreateClient("MyApiClient");

                // Define the endpoint path
                var endpointPath = "/api/Admin/GetPendingWithdrawDetail"; // Replace with the actual endpoint path

                // Prepare the content with parameters
                var requestData = new
                {
                    Email = CurrentUser(),
                    IdWithdraw = Id
                };

                var json = JsonConvert.SerializeObject(requestData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Make a POST request to the API
                var response = await client.PostAsync(endpointPath, content);
                var jsonResponse = await response.Content.ReadAsStringAsync();

                // Deserialize the JSON response into a List of Products
                var responseData = JsonConvert.DeserializeObject<WithdrawDetail>(jsonResponse);
                responseData.BankAccount = EncryptDecrypt.Decrypt(responseData.BankAccount);
                responseData.IFSCCode = EncryptDecrypt.Decrypt(responseData.IFSCCode);

                return Json(responseData);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return Json(null);
            }
        }

        [HttpGet] // Change from [HttpGet] to [HttpPost]
        public JsonResult UpdateRecharge(int id,int status)
        {
            var client = _httpClientFactory.CreateClient("MyApiClient");

            // Define the endpoint path
            var endpointPath = "/api/Admin/UpdateRechargeById";

            // Prepare the content with parameters
            var requestData = new
            {
                Email = CurrentUser(),
                IdHistory = id,
                Status = status
            };

            var json = JsonConvert.SerializeObject(requestData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Make a POST request to the API
            var response = client.PostAsync(endpointPath, content).Result;
            var jsonResponse = response.Content.ReadAsStringAsync().Result;

            // Deserialize the JSON response into an object
            var result = JsonConvert.DeserializeObject<Common>(jsonResponse);

            return Json(result);
        }

        [HttpGet] // Change from [HttpGet] to [HttpPost]
        public JsonResult UpdateWithdraw(int id, int status)
        {
            var client = _httpClientFactory.CreateClient("MyApiClient");

            // Define the endpoint path
            var endpointPath = "/api/Admin/UpdateWithdrawById";

            // Prepare the content with parameters
            var requestData = new
            {
                Email = CurrentUser(),
                IdWithdraw = id,
                Status = status
            };

            var json = JsonConvert.SerializeObject(requestData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Make a POST request to the API
            var response = client.PostAsync(endpointPath, content).Result;
            var jsonResponse = response.Content.ReadAsStringAsync().Result;

            // Deserialize the JSON response into an object
            var result = JsonConvert.DeserializeObject<Common>(jsonResponse);

            return Json(result);
        }
    }
}
