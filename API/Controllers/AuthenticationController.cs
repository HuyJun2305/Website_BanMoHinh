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

namespace XuongTT_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly CartRepo _cartRepo;
        private readonly IConfiguration _configuration;
        public AuthenticationController(UserManager<ApplicationUser> userManager, IConfiguration configuration, CartRepo cartRepo)
        {
            _userManager = userManager;
            _configuration = configuration;
            _cartRepo = cartRepo;
        }

        [HttpPost("Register")]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register([FromBody] DangKyModel model)
        {
            var existingUser = await _userManager.FindByNameAsync(model.Username);
            if (existingUser != null) return Conflict("User đã tồn tại.");

            var newUser = new ApplicationUser { UserName = model.Username ,
                                        Cart = new Cart(),
                                        Addresses = new List<Address>(),
                                        Email =model.Email ,
                                        SecurityStamp = Guid.NewGuid().ToString()
                                      };

            var result = await _userManager.CreateAsync(newUser,model.Password);
            _userManager.AddToRoleAsync(newUser, "Customer");
            var cart = new Cart
            {
                Id = Guid.NewGuid(),
                AccountId = newUser.Id,
                Price = 0
            };
            _cartRepo.Create(cart);
            if (result.Succeeded) return Ok("User tạo thành công");
            else return StatusCode(StatusCodes.Status500InternalServerError,$"Chưa tạo thành công user :{string.Join(" ", result.Errors.Select(e => e.Description))}");

        }
        [HttpPost("Login")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);

            if (user == null || !await _userManager.CheckPasswordAsync(user,model.Password)) return Unauthorized();

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, model.Username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration["JWT:Secret"] ?? throw new InvalidOperationException("Khóa bí mật chưa đc tạo ")));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.UtcNow.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
                );

            return Ok(new LoginResponse
            { JwtToken = new JwtSecurityTokenHandler().WriteToken(token),
            Expiration = token.ValidTo});

        }
    }
}
