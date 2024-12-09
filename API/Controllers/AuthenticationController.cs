using API.IRepositories;
using Data.Authentication;
using Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API.Controllers
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
        public async Task<IActionResult> Register([FromBody] Sign_In_Up_ViewModel model)
        {
            _logger.LogInformation("call register");

            // Kiểm tra username đã tồn tại
            var existingUser = await _userManager.FindByNameAsync(model.Username);
            if (existingUser != null) return Conflict("User đã tồn tại.");

            // Tạo tài khoản mới
            var newUser = new ApplicationUser
            {
                UserName = model.Username,
                Addresses = new List<Address>(),
                Email = model.Email,
                Name = model.FullName,
                PhoneNumber = model.PhoneNumber,
                SecurityStamp = Guid.NewGuid().ToString()

            };

            // Thực hiện tạo tài khoản
            var result = await _userManager.CreateAsync(newUser, model.Password);
            if (!result.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Chưa tạo thành công user: {string.Join(" ", result.Errors.Select(e => e.Description))}");
            }

            // Gán role mặc định là Customer
            var roleResult = await _userManager.AddToRoleAsync(newUser, "Customer");
            if (!roleResult.Succeeded)
            {
                _logger.LogError($"Không thể gán role: {string.Join(", ", roleResult.Errors.Select(e => e.Description))}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Không thể gán role Customer cho user.");
            }

            // Tạo giỏ hàng cho user sau khi tạo tài khoản thành công


            try
            {
                var cart = new Cart
                {
                    Id = Guid.NewGuid(),
                    AccountId = newUser.Id, // Gán AccountId sau khi user được tạo
                };

                var cartResult = await _cartRepo.CreateAsync(cart);
                if (cartResult == null)
                {
                    _logger.LogError("Không thể tạo giỏ hàng.");
                    return StatusCode(StatusCodes.Status500InternalServerError, "Không thể tạo giỏ hàng.");
                }

                // Gán giỏ hàng vào người dùng
                newUser.Cart = cart;

                // Lưu lại giỏ hàng vào cơ sở dữ liệu và cập nhật thông tin người dùng
                var userUpdateResult = await _userManager.UpdateAsync(newUser);
                if (!userUpdateResult.Succeeded)
                {
                    _logger.LogError($"Không thể cập nhật user: {string.Join(", ", userUpdateResult.Errors.Select(e => e.Description))}");
                    return StatusCode(StatusCodes.Status500InternalServerError, "Lỗi khi cập nhật thông tin user.");
                }

                _logger.LogInformation("Tạo giỏ hàng thành công.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Không thể tạo giỏ hàng: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Lỗi khi tạo giỏ hàng.");
            }

            _logger.LogInformation("register thành công");
            return Ok("User tạo thành công");
        }



        [HttpPost("Login")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            _logger.LogInformation("call Login");

            // Kiểm tra người dùng và mật khẩu
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
            {
                _logger.LogWarning("Sai tên đăng nhập hoặc mật khẩu.");
                return Unauthorized("Tên đăng nhập hoặc mật khẩu không đúng.");
            }

            // Lấy roles của người dùng
            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault(); // Lấy vai trò đầu tiên

            // Tạo refreshToken nếu chưa có
            var refreshToken = await _userManager.GetAuthenticationTokenAsync(user, "MyApp", "RefreshToken");
            if (string.IsNullOrEmpty(refreshToken))
            {
                refreshToken = Guid.NewGuid().ToString();
                await _userManager.SetAuthenticationTokenAsync(user, "MyApp", "RefreshToken", refreshToken);
            }

            // Tạo JWT token
            JwtSecurityToken token = await GenerateJwt(user.UserName, roles);

            _logger.LogInformation("Login Succeed");

            return Ok(new LoginResponse
            {
                JwtToken = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = token.ValidTo,
                RefreshToken = refreshToken
            });
        }



        [HttpPost("Refresh")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Refresh([FromBody] RefreshModel model)
        {
            _logger.LogInformation("call refresh");

            // Lấy principal từ access token hết hạn
            var principal = GetPrincipalFromExpiredToken(model.AccessToken);
            if (principal?.Identity?.Name is null)
            {
                return Unauthorized("Access token không hợp lệ hoặc đã hết hạn.");
            }

            // Kiểm tra người dùng có tồn tại không
            var user = await _userManager.FindByNameAsync(principal.Identity.Name);
            if (user == null)
            {
                return Unauthorized("Người dùng không tồn tại.");
            }

            // Kiểm tra refresh token có hợp lệ không
            var refreshToken = await _userManager.GetAuthenticationTokenAsync(user, "MyApp", "RefreshToken");
            var isValid = await _userManager.VerifyUserTokenAsync(user, "MyApp", "RefreshToken", model.RefreshToken);

            if (!isValid || refreshToken != model.RefreshToken)
            {
                return Unauthorized("Refresh token không hợp lệ.");
            }

            // Tạo JWT mới
            var roles = await _userManager.GetRolesAsync(user);
            var token = await GenerateJwt(user.UserName, roles);

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
            _logger.LogInformation("Logout");

            var username = HttpContext.User.Identity?.Name;
            if (string.IsNullOrEmpty(username)) return Unauthorized("Không tìm thấy người dùng.");

            var user = await _userManager.FindByNameAsync(username);
            if (user == null) return Unauthorized("Người dùng không hợp lệ.");

            await _userManager.UpdateSecurityStampAsync(user);

            _logger.LogInformation("Logout succeed");
            return Ok("Đăng xuất thành công.");
        }

        private ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token)
        {
            // Kiểm tra token null hoặc rỗng
            if (string.IsNullOrEmpty(token))
            {
                throw new ArgumentNullException(nameof(token), "Token không được null hoặc rỗng");
            }

            // Lấy khóa bí mật từ cấu hình
            var secret = _configuration["JWT:Secret"] ?? throw new InvalidOperationException("Khóa bí mật chưa được tạo");

            var validation = new TokenValidationParameters
            {
                ValidIssuer = _configuration["JWT:ValidIssuer"], // Đảm bảo tên khóa trong cấu hình chính xác
                ValidAudience = _configuration["JWT:ValidAudience"], // Đảm bảo tên khóa trong cấu hình chính xác
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)),
                ValidateLifetime = false // Không kiểm tra thời gian sống của token, vì chúng ta đang xác thực token hết hạn
            };

            try
            {
                // Xác thực token và trả về ClaimsPrincipal
                var principal = new JwtSecurityTokenHandler().ValidateToken(token, validation, out var validatedToken);

                // Bạn có thể kiểm tra thêm thông tin từ validatedToken nếu cần
                if (validatedToken is JwtSecurityToken jwtToken)
                {
                    // Xử lý thêm nếu cần, ví dụ như kiểm tra thông tin của jwtToken
                }

                return principal;
            }
            catch (SecurityTokenException)
            {
                // Xử lý ngoại lệ khi token không hợp lệ
                return null; // Trả về null hoặc xử lý lỗi tùy theo yêu cầu của ứng dụng
            }
        }



        private async Task<JwtSecurityToken> GenerateJwt(string username, IEnumerable<string> roles)
        {
            // Đảm bảo gọi FindByNameAsync một cách đồng bộ
            var user = await _userManager.FindByNameAsync(username);

            if (user == null)
            {
                throw new InvalidOperationException("Người dùng không tồn tại.");
            }

            // Tạo danh sách các claims
            var authClaims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, username),
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), // Thêm NameIdentifier từ Id của người dùng
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    };

            // Thêm các vai trò vào claims
            foreach (var role in roles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            // Lấy khóa bí mật từ cấu hình
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration["JWT:Secret"] ?? throw new InvalidOperationException("Khóa bí mật chưa được tạo")));

            // Tạo JWT Token
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
