using System.Security.Claims;
using LcvFlow.Domain;
using LcvFlow.Service.Dtos.Auth;
using LcvFlow.Service.Helpers.Auth;
using LcvFlow.Service.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MyApp.Namespace
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromForm] string username, [FromForm] string password)
        {
            var result = await _authService.ValidateUserAsync(username, password);
            if (!result.IsSuccess)
                return LocalRedirect($"/admin/login?error=error");

            var claims = new List<Claim>
            {   
                new(ClaimTypes.Name, result.Data.Username),
                new(ClaimTypes.NameIdentifier, result.Data.Id.ToString())
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity));

            return LocalRedirect("/admin/dashboard");
        }

        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
            // Cookie şemasını belirterek oturumu sonlandırıyoruz
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Çıkış yaptıktan sonra ana sayfaya (veya login'e) yönlendiriyoruz
            return LocalRedirect("/");
        }
    }
}