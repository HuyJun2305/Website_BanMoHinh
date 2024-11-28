using Data.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Data.Models;
using API.Data;
using API.Repositories;
using API.IRepositories;
using Microsoft.AspNetCore.Authorization;

namespace XuongTT_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICartRepo _cartRepo;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthenticationController> _logger;
        public AuthenticationController(UserManager<ApplicationUser> userManager, IConfiguration configuration, ICartRepo cartRepo, ILogger<AuthenticationController> logger)
        {
            _userManager = userManager;
            _configuration = configuration;
            _cartRepo = cartRepo;
            _logger = logger;
        }

        [HttpPost("Register")]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register([FromBody] DangKyModel model)
        {
            _logger.LogInformation("call register");

            var existingUser = await _userManager.FindByNameAsync(model.Username);
            if (existingUser != null) return Conflict("User đã tồn tại.");

            var newUser = new ApplicationUser { UserName = model.Username ,
                                        Addresses = new List<Address>(),
                                        Email =model.Email ,
                                        SecurityStamp = Guid.NewGuid().ToString()
                                      };

            var cart = new Cart
            {
                Id = Guid.NewGuid(),
                AccountId = newUser.Id,
            };
            _cartRepo.Create(cart);

            newUser.Cart = cart;

            var result = await _userManager.CreateAsync(newUser, model.Password);
            _userManager.AddToRoleAsync(newUser, "Customer");
            if (result.Succeeded)
            {
                _logger.LogInformation("register thành công");
                return Ok("User tạo thành công"); 
            }
            else return StatusCode(StatusCodes.Status500InternalServerError,$"Chưa tạo thành công user :{string.Join(" ", result.Errors.Select(e => e.Description))}");

        }
        [HttpPost("Login")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            _logger.LogInformation("call Login");

            var user = await _userManager.FindByNameAsync(model.Username);

            if (user == null || !await _userManager.CheckPasswordAsync(user,model.Password)) return Unauthorized();

            JwtSecurityToken token = GenerateJwt(model.Username);

            _logger.LogInformation("Login Succeed");

            return Ok(new LoginResponse
            { JwtToken = new JwtSecurityTokenHandler().WriteToken(token),
            Expiration = token.ValidTo});

        }

        [HttpPost("Refresh")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Refresh([FromBody] RefreshModel model)
        {
            _logger.LogInformation("call refresh");

            var principal = GetPrincipalFromExpiredToken(model.AccessToken);

            if (principal?.Identity?.Name is null) return Unauthorized();

            var user = await _userManager.FindByNameAsync(principal.Identity.Name);

            var refreshToken = await _userManager.GetAuthenticationTokenAsync(user, "MyApp", "RefeshToken");
            var isValid = await _userManager.VerifyUserTokenAsync(user, "MyApp", "RefeshToken", model.RefreshToken);

            if (user is null || !isValid) return Unauthorized();

            var token = GenerateJwt(principal.Identity.Name);

            _logger.LogInformation("Refresh succeed");
            return Ok(new LoginResponse
            {
                JwtToken = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = token.ValidTo,
                RefreshToken = refreshToken
            });
        }

        [Authorize]
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Logout()
        {
            _logger.LogInformation($"Logout");
            var username = HttpContext.User.Identity?.Name;

            var user = await _userManager.FindByNameAsync(username);

            if (username == null || user is null) return Unauthorized();


            _userManager.UpdateSecurityStampAsync(user);

            _logger.LogInformation($"Logout succeed");
            return Ok();
        }

        private ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token)
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


        private JwtSecurityToken GenerateJwt(string username)
        {
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration["JWT:Secret"] ?? throw new InvalidOperationException("Khóa bí mật chưa đc tạo ")));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.UtcNow.AddMinutes(30),
                claims: authClaims,
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
                );
            return token;
        }




    }
}
