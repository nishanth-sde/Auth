using JWT.Auth.Data;
using JWT.Auth.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;

namespace JWT.Auth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IHttpClientFactory _httpClientFactory;
        public AuthController(IAuthService authService, IHttpClientFactory httpClientFactory)
        {
            _authService = authService;
            _httpClientFactory = httpClientFactory;
        }

        [HttpPost]
        public async Task <IActionResult> Login(string email,string password)
        {
            var token = await _authService.LoginAsync(email, password);
            return Ok(token);

        }
        [Authorize]
        [HttpGet]
        public IActionResult Test()
        {
            return Ok("Test successful");
        }

        [HttpGet]
        [Route("GetOrder")]
        public async Task<IActionResult> GetOrder()
        {
            var client = _httpClientFactory.CreateClient();

            var response = await client.GetAsync(
                $"https://localhost:44371/odata/Product");

            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode,
                    "Unable to get user details");
            }

            var userJson = await response.Content.ReadAsStringAsync();

            return Ok(userJson);
        }


        [HttpPost("/Hash/Password")]
        public string HashPass(string password)
        {
            var hashedpassword = _authService.HashPassword(password);
            return hashedpassword;

        }
    }
}
