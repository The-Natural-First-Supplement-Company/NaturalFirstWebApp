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
using Newtonsoft.Json.Linq;

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

        public IActionResult Recharge()
        {
            Decimal value = GetWalletAmount();
            TempData["Wallet"] = value;
            return View();
        }

        [HttpPost]
        public IActionResult Recharge(int Amount,string paymentOption)
        {
            TempData["Amount"] = Amount;
            TempData["PayOption"] = paymentOption;
            return RedirectToAction("MakePayment");
        }

        public IActionResult MakePayment()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> MakePayment(RechargeVM recharge)
        {
            decimal.TryParse(TempData["Amount"].ToString(), out decimal amt);
            string option = TempData["PayOption"].ToString();

            recharge.Amount = amt;
            recharge.PayOption = option;
            try
            {
                // Create an instance of HttpClient using the named client from the factory
                var client = _httpClientFactory.CreateClient("MyApiClient");

                // Define the endpoint path
                var endpointPath = "/api/User/RechargeWallet"; // Replace with the actual login endpoint path

                // Prepare the content with parameters
                var requestData = new 
                { 
                    Email = CurrentUser(), 
                    Amount = recharge.Amount,
                    PayOption = recharge.PayOption,
                    PayCode = recharge.PayCode
                };
                var json = JsonConvert.SerializeObject(requestData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Make a POST request to the API
                var response = await client.PostAsync(endpointPath, content);
                var jsonResponse = await response.Content.ReadAsStringAsync();
                // Deserialize the JSON response into an object
                var responseData = JsonConvert.DeserializeObject<Common>(jsonResponse);

                if (response.IsSuccessStatusCode)
                {
                    ViewBag.msg = responseData.Status;
                    return View();
                }
                else
                {
                    ViewBag.msg = responseData.Status;
                    return View();
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View();
            }
        }

        public IActionResult RechargeHistory()
        {
            // Retrieve the user's claims
            var userClaims = User.Claims.ToList();

            // Retrieve specific claim values
            ViewBag.Email = userClaims[0].Value;
            return View();
        }

        public IActionResult Withdraws()
        {
            BankDetails bank = new BankDetails();
            bank = GetBankDetails();
            ViewBag.Balance = GetBalanceAmount();
            return View(bank);
        }

        public IActionResult WithdrawHistory()
        {
            // Retrieve specific claim values
            ViewBag.Email = CurrentUser();
            return View();
        }

        public IActionResult Salary()
        {
            return View();
        }
        
        //Me Options
        public async Task<IActionResult> Profile()
        {
            try
            {
                // Create an instance of HttpClient using the named client from the factory
                var client = _httpClientFactory.CreateClient("MyApiClient");

                //client.DefaultRequestHeaders.Add("Token","");

                // Define the endpoint path
                var endpointPath = "/api/User/GetProfileInfor"; // Replace with the actual login endpoint path

                // Prepare the content with parameters
                var requestData = new
                {
                    Id = GetCurrentUserId()
                };
                var json = JsonConvert.SerializeObject(requestData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Make a POST request to the API
                var response = await client.PostAsync(endpointPath, content);
                var jsonResponse = await response.Content.ReadAsStringAsync();
                // Deserialize the JSON response into an object
                var responseData = JsonConvert.DeserializeObject<UserProfile>(jsonResponse);

                return View(responseData);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Unable to fetch information. Please try again later.";
                return View(null);
            }
        }

        public IActionResult AddBank()
        {
            if (BankExistsInDB())
            {
                BankDetails bank = new BankDetails();
                bank = GetBankDetails();
                return View(bank);
            }
            else
            {
                if (TempData["Message"] != null)
                {
                    ViewBag.Message = TempData["Message"];
                    TempData.Remove("Message");
                }
                if (TempData["ClearForm"] != null)
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
                        AccountNo = EncryptDecrypt.Encrypt(bank.AccountNo),
                        IFSCCode = EncryptDecrypt.Encrypt(bank.IFSCCode),
                        RealName = bank.RealName,
                        TrnPassword = EncryptDecrypt.Encrypt(bank.TrnPassword),
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
        public IActionResult PasswordReset()
        {
            try
            {
                string Email = CurrentUser();
                ViewData["Email"] = Email;
                return View();
            }catch(Exception e)
            {
                return RedirectToAction("Index","Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> PasswordReset(ResetPassword reset)
        {
            string Email = CurrentUser();
            ViewData["Email"] = Email;
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
            else
            {
                return View(reset);
            }
        }

        //Reset Transaction Password
        public IActionResult PaymentReset()
        {
            string Email = CurrentUser();
            ViewData["Email"] = Email;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PaymentReset(ResetPassword reset)
        {
            string Email = CurrentUser();
            ViewData["Email"] = Email;
            ResetPassword rp = new ResetPassword();
            if (ModelState.IsValid && reset.VerificationCode == TempData["VerificationCode"].ToString())
            {
                try
                {
                    // Create an instance of HttpClient using the named client from the factory
                    var client = _httpClientFactory.CreateClient("MyApiClient");

                    // Define the endpoint path
                    var endpointPath = "/api/User/ResetPaymentPassword"; // Replace with the actual login endpoint path

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
                    return View(reset);
                }
            }
            else
            {
                return View(reset);
            }
        }

        public IActionResult MyTeam()
        {
            return View();
        }

        public IActionResult Income()
        {
            return View();
        }

        public IActionResult IncomeHistory()
        {
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Home"); // Redirect to a specific page after logout
        }

        /*
         
        Methods which require calling for showing data
         
         */

        public JsonResult GetMyTeam([FromQuery]int perc)
        {
            var client = _httpClientFactory.CreateClient("MyApiClient");

            // Define the endpoint path
            var endpointPath = "/api/User/GetMyTeam";

            // Prepare the content with parameters
            var requestData = new
            {
                Percent = perc,
                UserId = GetCurrentUserId()
            };
            var json = JsonConvert.SerializeObject(requestData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Make a POST request to the API
            var response = client.PostAsync(endpointPath, content).Result;
            var jsonResponse = response.Content.ReadAsStringAsync().Result;

            // Deserialize the JSON response into an object
            var result = JsonConvert.DeserializeObject<List<MyTeamVM>>(jsonResponse);
            return Json(result);
        }

        [HttpPost]
        public JsonResult MakeWithdraws([FromBody] WithdrawVM draw)
        {
            var client = _httpClientFactory.CreateClient("MyApiClient");

            // Define the endpoint path
            var endpointPath = "/api/User/AddWithdrawRequest";

            // Prepare the content with parameters
            var requestData = new
            {
                Email = CurrentUser(),
                Amount = draw.Amount,
                TrnPassword = EncryptDecrypt.Encrypt(draw.TrnPassword)
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

        public BankDetails GetBankDetails()
        {
            BankDetails responseData = new BankDetails();
            try
            {
                // Create an instance of HttpClient using the named client from the factory
                var client = _httpClientFactory.CreateClient("MyApiClient");

                // Define the endpoint path
                var endpointPath = "/api/User/GetBankDetails";

                // Prepare the content with parameters
                var requestData = new
                {
                    Email = CurrentUser()
                };
                var json = JsonConvert.SerializeObject(requestData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Make a POST request to the API
                var response = client.PostAsync(endpointPath, content).Result; // Using .Result
                var jsonResponse = response.Content.ReadAsStringAsync().Result;

                // Deserialize the JSON response into an object
                responseData = JsonConvert.DeserializeObject<BankDetails>(jsonResponse);
                responseData.AccountNo = EncryptDecrypt.Decrypt(responseData.AccountNo);
                responseData.IFSCCode = EncryptDecrypt.Decrypt(responseData.IFSCCode);

            }
            catch (Exception ex)
            {
                ViewBag.Error = "Unable to fetch information. Please try again later.";
            }
            return responseData;
        }
        //Recharge History
        public async Task<JsonResult> GetHistory()
        {
            try
            {
                // Create an instance of HttpClient using the named client from the factory
                var client = _httpClientFactory.CreateClient("MyApiClient");

                // Define the endpoint path
                var endpointPath = "/api/User/GetRechargeHistory"; // Replace with the actual login endpoint path

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
                var responseData = JsonConvert.DeserializeObject<List<RechargeHistory>>(jsonResponse);

                return Json(responseData);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return Json(null);
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetPendingIncome()
        {
            try
            {
                // Create an instance of HttpClient using the named client from the factory
                var client = _httpClientFactory.CreateClient("MyApiClient");

                // Define the endpoint path
                var endpointPath = "/api/User/FetchPendingIncome"; // Replace with the actual login endpoint path

                // Prepare the content with parameters
                var requestData = new
                {
                    Id = GetCurrentUserId()
                };
                var json = JsonConvert.SerializeObject(requestData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Make a POST request to the API
                var response = await client.PostAsync(endpointPath, content);
                var jsonResponse = await response.Content.ReadAsStringAsync();
                // Deserialize the JSON response into an object
                var responseData = JsonConvert.DeserializeObject<List<IncomeVM>>(jsonResponse);

                return Json(responseData);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return Json(null);
            }
        }

        public async Task<JsonResult> GetHistoryWithdrawal()
        {
            try
            {
                // Create an instance of HttpClient using the named client from the factory
                var client = _httpClientFactory.CreateClient("MyApiClient");

                // Define the endpoint path
                var endpointPath = "/api/User/GetWithdarwsUser"; // Replace with the actual login endpoint path

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
                var responseData = JsonConvert.DeserializeObject<List<Withdraw>>(jsonResponse);

                return Json(responseData);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return Json(null);
            }
        }

        [HttpPost]
        public async Task<JsonResult> UpdateIncomeStatus([FromBody] IncomeVM income)
        {
            try
            {
                // Create an instance of HttpClient using the named client from the factory
                var client = _httpClientFactory.CreateClient("MyApiClient");

                // Define the endpoint path
                var endpointPath = "/api/User/UpdateIncomeReceive"; // Replace with the actual login endpoint path


                // Prepare the content with parameters
                var requestData = new {
                    UserId = GetCurrentUserId(),
                    wbHistoryId = income.wbHistoryId
                };
                var json = JsonConvert.SerializeObject(requestData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Make a POST request to the API
                var response = await client.PostAsync(endpointPath, content);
                var jsonResponse = await response.Content.ReadAsStringAsync();
                // Deserialize the JSON response into an object
                var responseData = JsonConvert.DeserializeObject<Common>(jsonResponse);

                return Json(responseData);
            }
            catch (Exception ex)
            {
                ViewBag.StatudId = 0;
                ViewBag.msg = ex.Message;
                return Json(null);
            }
        }

        // Get Wallet Amount
        public Decimal GetWalletAmount()
        {
            var client = _httpClientFactory.CreateClient("MyApiClient");

            // Define the endpoint path
            var endpointPath = "/api/User/GetWalletBalance"; // Replace with the actual login endpoint path

            // Prepare the content with parameters
            var requestData = new
            {
                Email = CurrentUser()
            };
            var json = JsonConvert.SerializeObject(requestData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Make a POST request to the API
            var response = client.PostAsync(endpointPath, content).Result;
            var jsonResponse = response.Content.ReadAsStringAsync().Result;

            // Deserialize the JSON response into an object
            var responseData = JsonConvert.DeserializeObject<Decimal>(jsonResponse);

            return responseData;
        }

        public Decimal GetBalanceAmount()
        {
            var client = _httpClientFactory.CreateClient("MyApiClient");

            // Define the endpoint path
            var endpointPath = "/api/User/GetBalanceAmount"; // Replace with the actual login endpoint path

            // Prepare the content with parameters
            var requestData = new
            {
                Email = CurrentUser()
            };
            var json = JsonConvert.SerializeObject(requestData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Make a POST request to the API
            var response = client.PostAsync(endpointPath, content).Result;
            var jsonResponse = response.Content.ReadAsStringAsync().Result;

            // Deserialize the JSON response into an object
            var responseData = JsonConvert.DeserializeObject<Decimal>(jsonResponse);

            return responseData;
        }

        public bool BankExistsInDB()
        {
            var client = _httpClientFactory.CreateClient("MyApiClient");

            // Define the endpoint path
            var endpointPath = "/api/User/BankExistsInDB"; // Replace with the actual login endpoint path

            // Prepare the content with parameters
            var requestData = new
            {
                Email = CurrentUser()
            };
            var json = JsonConvert.SerializeObject(requestData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Make a POST request to the API
            var response = client.PostAsync(endpointPath, content).Result;
            var jsonResponse = response.Content.ReadAsStringAsync().Result;

            // Deserialize the JSON response into an object
            var responseData = JsonConvert.DeserializeObject<bool>(jsonResponse);

            return responseData;
        }

        public Decimal ActualBalance()
        {
            var client = _httpClientFactory.CreateClient("MyApiClient");

            // Define the endpoint path
            var endpointPath = "/api/User/BankExistsInDB"; // Replace with the actual login endpoint path

            // Prepare the content with parameters
            var requestData = new
            {
                Email = CurrentUser()
            };
            var json = JsonConvert.SerializeObject(requestData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Make a POST request to the API
            var response = client.PostAsync(endpointPath, content).Result;
            var jsonResponse = response.Content.ReadAsStringAsync().Result;

            // Deserialize the JSON response into an object
            var responseData = JsonConvert.DeserializeObject<Decimal>(jsonResponse);

            return responseData;
        }
    }
}
