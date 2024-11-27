using Data.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using View.IServices;

namespace View.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly IAuthenticationService _authenticationService;
        public event Action<string?>? LoginChange;

        public AuthenticationController( IAuthenticationService authenticationService )
        {
            _authenticationService = authenticationService;
        }
        public async Task<IActionResult> Login()
        {
            return View();
        }
        //[HttpPost]
        //public async Task<IActionResult> Login(LoginModel model)
        //{
        //    // Gọi dịch vụ xác thực để lấy JWT
        //    LoginResponse login = await _authenticationService.LoginAsync(model);

        //    // Kiểm tra nếu đăng nhập không thành công
        //    if (login == null)
        //        throw new UnauthorizedAccessException();

        //    // Lưu JWT vào session
        //    HttpContext.Session.SetString("JWT", login.JwtToken);

        //    // Giải mã JWT để lấy thông tin role
        //    var handler = new JwtSecurityTokenHandler();
        //    var jwtToken = handler.ReadJwtToken(login.JwtToken);

        //    // Lấy role từ claim
        //    var role = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

        //    // Kiểm tra role và chuyển hướng
        //    if (role == "Admin" || role == "Staff")
        //    {
        //        return RedirectToAction("Index", "Home");
        //    }
        //    else if (role == "Customer")
        //    {
        //        return RedirectToAction("Index", "HomeCustomer");
        //    }

        //    // Nếu không xác định được role, xử lý mặc định
        //    return RedirectToAction("Login", "Authentication");
        //}

        [HttpDelete]
        public async Task<IActionResult> Logout()
        {
            ViewBag.Expiration = null;
            await _authenticationService.LogoutAsync();
            return RedirectToAction("Index", "Home");
        }




    }
}
