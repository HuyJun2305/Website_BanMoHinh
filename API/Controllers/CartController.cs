using API.IRepositories;
using API.Repositories;
using Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Controller;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
	{
        private readonly ICartRepo _cartRepo;
		private readonly ILogger<CartController> _logger;
		public CartController(ICartRepo cartRepo, ILogger<CartController> logger)
		{
			_cartRepo = cartRepo;
			_logger = logger;
		}
		[HttpGet]
        public async Task<ActionResult<IEnumerable<Cart>>> GetCart()
        {
            try
            {
                return await _cartRepo.GetAllCart();
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
        //
        [HttpGet("GetCartById")]
        public async Task<ActionResult<Cart>> GetByIdCart(Guid id)
        {
            try
            {
                return await _cartRepo.GetCartById(id);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
        [HttpGet("GetCartByUserId")]
        public async Task<ActionResult<Cart>> GetCartByUserId(Guid userId)
        {
            try
            {
                return await _cartRepo.GetCartByUserId(userId);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        //Add product
        [HttpPost]
        public async Task<ActionResult<Cart>> PostCart(Cart cart)
        {
            try
            {
                Cart ct = new Cart()
                {
                    Id = cart.Id,
                    Account = cart.Account,
                    AccountId = cart.AccountId

                };
                await _cartRepo.Create(ct);
                await _cartRepo.SaveChanges();
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
            return Content("Success!");
        }
        //
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCart(Cart cart)
        {
            try
            {
                Cart ct = new Cart()
                {
                    Id = cart.Id,
                    Account = cart.Account,
                    AccountId = cart.AccountId

                };
                await _cartRepo.Create(ct);
                await _cartRepo.SaveChanges();
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
            return NoContent();
        }
        [HttpGet("GetCartForLayout")]
        public async Task<IActionResult> GetCartByForLayout()
        {
			// Lấy userId từ Claims
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var userGuid))
            {
                return Unauthorized();
            }

            // Lấy giỏ hàng
            var cart = await _cartRepo.GetCartByUserId(userGuid);
            if (cart == null)
            {
                return Ok(new { Items = new List<object>(), Total = 0 });
            }

            // Tạo kết quả trả về
            var result = new
            {
                Items = cart.CartDetails.Select(ci => new
                {
                    ci.Product.Name,
                    ImageUrl = ci.Product.Images.FirstOrDefault()?.URL ?? "default-image-url",
                    ci.Quantity,
                    Price = ci.Product.Price * ci.Quantity
                }),
                Total = cart.CartDetails.Sum(ci => ci.Product.Price * ci.Quantity)
            };

            return Ok(result);
        }


    }
}
