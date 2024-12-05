using Data.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
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

			// Kiểm tra nếu token không null
			if (jwt != null)
			{
				HttpContext.Session.SetString("jwtToken", jwt);
				// Lấy principal từ token (claims)

				var principal = GetPrincipalFromToken(jwt);
				await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
				// Lấy role từ token
				var role = GetRoleName(jwt);

				if (role == null)
				{
					throw new UnauthorizedAccessException("Không có quyền truy cập.");
				}
				var username = model.Username;

				var userId = GetUserId(jwt);

				// Lưu tên người dùng vào session
				HttpContext.Session.SetString("userId", userId);
				// Lưu tên người dùng vào session
				HttpContext.Session.SetString("username", username);
				// Lưu role vào session
				HttpContext.Session.SetString("role", role);

				// Điều hướng đến trang chính tùy thuộc vào vai trò của người dùng
				if (role == "Staff" || role == "Admin")
				{
					// Nếu là Staff hoặc Admin, chuyển đến trang Home
					return RedirectToAction("Index", "Home");
				}
				else if (role == "Customer" || role == "Guest")
				{
					// Nếu là Customer hoặc Guest, chuyển đến trang HomeCustomer
					return RedirectToAction("Index", "HomeCustomer");
				}
				else
				{
					// Nếu không phải là những role trên, có thể điều hướng về trang lỗi hoặc thông báo lỗi
					return Unauthorized();
				}
			}
			else
			{
				// Nếu token trả về null, hiển thị thông báo lỗi
				ViewData["LoginError"] = "Tên đăng nhập hoặc mật khẩu không đúng.";
				return View(model); // Trả về lại trang login với thông báo lỗi
			}
		}

		public async Task<IActionResult> Register()
        {
            return View();
        }
		[HttpPost]
		public async Task<IActionResult> Register(DangKyModel model)
		{
			
			var regis = await _authenticationService.Register(model);

            // Kiểm tra nếu token không null
            if (!regis)
            {
				ViewData["RegisterError"] = "chưa đăng kí thành công";
				return View(model);
            }
			return RedirectToAction("Login","Authentication");
        }

		private static string GetUserId(string token)
		{
			var jwt = new JwtSecurityToken(token);
			return jwt.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
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
		// Action Logout
		[HttpPost]
		public async Task<IActionResult> Logout()
		{
			// Hủy đăng nhập của người dùng
			await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

			// Xóa thông tin session (nếu có)
			HttpContext.Session.Clear();

			// Chuyển hướng về trang đăng nhập
			return RedirectToAction("Login", "Authentication");
		}




	}
}
