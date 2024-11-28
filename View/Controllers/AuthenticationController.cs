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
        public AuthenticationController( IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }
        public async Task<IActionResult> Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            string role = await _authenticationService.LoginAsync(model);
            if (role == null) throw new UnauthorizedAccessException();
            var username = model.Username;
            HttpContext.Session.SetString("username", username);
            if (role != "Customer")
            {
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Index","HomeCustomer");
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
