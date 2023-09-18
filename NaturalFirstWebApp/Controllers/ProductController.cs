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
using System.Net.Http.Headers;

namespace NaturalFirstWebApp.Controllers
{
    public class ProductController : CommonController
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public ProductController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [AuthorizationFilter(Roles = "User")]
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

        [AuthorizationFilter(Roles = "User")]
        public async Task<IActionResult> ProductDetails(int id)
        {
            if(id > 0) 
            {
                try
                {
                    // Create an instance of HttpClient using the named client from the factory
                    using (var client = _httpClientFactory.CreateClient("MyApiClient"))
                    {
                        // Define the endpoint path and include query parameters
                        var endpointPath = $"/api/Product/GetProductById?IdProducts={id}";

                        // Make a GET request to the API
                        var response = await client.GetAsync(endpointPath);

                        // Check if the request was successful (HTTP status code 2xx)
                        response.EnsureSuccessStatusCode();

                        var jsonResponse = await response.Content.ReadAsStringAsync();

                        // Deserialize the JSON response into an object
                        var responseData = JsonConvert.DeserializeObject<Products>(jsonResponse);

                        return View(responseData);
                    }
                }
                catch (HttpRequestException ex)
                {
                    // Handle HTTP request errors
                    ViewBag.Error = $"Error making HTTP request: {ex.Message}";
                    return View();
                }
                catch (Exception ex)
                {
                    // Handle other exceptions
                    ViewBag.Error = $"An error occurred: {ex.Message}";
                    return View();
                }
            }
            else
            {
                return RedirectToAction("Products","Product");
            }
        }

        [AuthorizationFilter(Roles = "User")]
        public async Task<IActionResult> GetPurchaseWallet()
        {
            try
            {
                // Create an instance of HttpClient using the named client from the factory
                using (var client = _httpClientFactory.CreateClient("MyApiClient"))
                {
                    // Define the endpoint path and include query parameters
                    var endpointPath = "/api/User/GetPDWallet";
                    
                    // Make a GET request to the API
                    var requestData = new
                    {
                        Email = CurrentUser(),
                    };
                    var json = JsonConvert.SerializeObject(requestData);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    // Make a POST request to the API
                    var response = await client.PostAsync(endpointPath, content);
                    var jsonResponse = await response.Content.ReadAsStringAsync();

                    // Deserialize the JSON response into an object
                    var responseData = JsonConvert.DeserializeObject<PDWallet>(jsonResponse);

                    return Json(responseData);
                }
            }
            catch (HttpRequestException ex)
            {
                // Handle HTTP request errors
                ViewBag.Error = $"Error making HTTP request: {ex.Message}";
                return Json(null);
            }
            catch (Exception ex)
            {
                // Handle other exceptions
                ViewBag.Error = $"An error occurred: {ex.Message}";
                return Json(null);
            }
        }

        [AuthorizationFilter(Roles = "User")]
        [HttpPost]
        public async Task<IActionResult> PurchaseProduct([FromBody]PDWallet prd)
        {
            try
            {
                // Create an instance of HttpClient using the named client from the factory
                var client = _httpClientFactory.CreateClient("MyApiClient");

                // Define the endpoint path
                var endpointPath = "api/Product/PurchaseProduct";

                // Prepare the content with parameters
                var requestData = new
                {
                    Email = CurrentUser(),
                    IdProducts = prd.ProductId
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
            catch (HttpRequestException ex)
            {
                // Handle HTTP request errors
                return Json(new { StatusId = 0, Status = ex.Message });
            }
            catch (Exception ex)
            {
                // Handle other exceptions
                return Json(new { StatusId = 0, Status = ex.Message });
            }
        }

        [AuthorizationFilter(Roles = "User")]
        
        public async Task<IActionResult> MyTeamProducts([FromQuery]int userId)
        {
            try
            {
                // Create an instance of HttpClient using the named client from the factory
                var client = _httpClientFactory.CreateClient("MyApiClient");

                // Define the endpoint path
                var endpointPath = "api/Product/GetMyTeamProducts";

                // Prepare the content with parameters
                var requestData = new
                {
                    Id = userId
                };

                var json = JsonConvert.SerializeObject(requestData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Make a POST request to the API
                var response = await client.PostAsync(endpointPath, content);
                var jsonResponse = await response.Content.ReadAsStringAsync();

                // Deserialize the JSON response into an object
                var responseData = JsonConvert.DeserializeObject<List<Products>>(jsonResponse);

                return View(responseData);
            }
            catch (HttpRequestException ex)
            {
                // Handle HTTP request errors
                return View();
            }
            catch (Exception ex)
            {
                // Handle other exceptions
                return View();
            }
        }
    }
}
