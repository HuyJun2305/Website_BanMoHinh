using Data.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using View.IServices;
using IAuthenticationService = View.IServices.IAuthenticationService;

namespace View.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly IAuthenticationService _authenticationService;
        public event Action<string?>? LoginChange;
        private readonly IConfiguration _configuration;

        public AuthenticationController(IAuthenticationService authenticationService, IConfiguration configuration)
        {
            _authenticationService = authenticationService;
            _configuration = configuration;
        }
        public async Task<IActionResult> Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            var jwt = await _authenticationService.LoginAsync(model);
            if (jwt != null)
            {
                var principal = GetPrincipalFromToken(jwt);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,principal);

                var role = GetRoleName(jwt);

                if (role == null) throw new UnauthorizedAccessException();
                var username = model.Username;
                HttpContext.Session.SetString("username", username);
                if (role != "Customer")
                {
                    return RedirectToAction("Index", "Home");
                }

                return RedirectToAction("Index", "HomeCustomer");
            }
            else
            {
                ViewData["LoginError"] = "Tên đăng nhập hoặc mật khẩu không đúng.";
                return View(model);
            }

        }
        private static string GetRoleName(string token)
        {
            var jwt = new JwtSecurityToken(token);

            return jwt.Claims.First(c => c.Type == ClaimTypes.Role).Value;
        }
        private ClaimsPrincipal? GetPrincipalFromToken(string? token)
        {
            var secret = _configuration["JWT:Secret"] ?? throw new InvalidOperationException("Khóa bí mật chưa đc tạo ");


            var validation = new TokenValidationParameters
            {
                ValidIssuer = _configuration["Jwt:ValidIssuer"],
                ValidAudience = _configuration["Jwt:ValidAudience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)),
                ValidateLifetime = false
            };

            return new JwtSecurityTokenHandler().ValidateToken(token, validation, out _);
        }
        [HttpDelete]
        public async Task<IActionResult> Logout()
        {
            ViewBag.Expiration = null;
            await _authenticationService.LogoutAsync();
            return RedirectToAction("Index", "Home");
        }




    }
}
