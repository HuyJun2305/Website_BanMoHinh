using API.IRepositories;
using API.Repositories;
using Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartDetailController : ControllerBase
    {
        private readonly ICartDetailRepo _cartRepo;
        private readonly IProductRepos _productRepo;

        public CartDetailController(ICartDetailRepo cartRepo)
        {
            _cartRepo = cartRepo;
        }
        [HttpGet("GetAllCartDetails")]
        public async Task<IActionResult> GetAllCartDetails()
        {
            var lstCartDetails = await _cartRepo.GetAllCartDetail();
            return Ok(lstCartDetails);
        }
        [HttpGet("GetCartDetailsByCartId")]
        public async Task<IActionResult> GetCartDetailByCartId(Guid cartId)
        {
            var lstCartDetails = await _cartRepo.GetCartDetailByCartId(cartId);
            if (lstCartDetails == null || lstCartDetails.Count == 0)
            {
                return NotFound();
            }
            return Ok(lstCartDetails);
        }
        [HttpGet("GetCartDetailsById")]
        public async Task<IActionResult> GetCartDetailById(Guid id)
        {
            var cartDetails = await _cartRepo.GetCartDetailById(id);
            if (cartDetails == null)
            {
                return NotFound();
            }
            return Ok(cartDetails);
        }
        [HttpGet("GetCartDetailByProductId")]
        public async Task<IActionResult> GetCartDetailByProductId(Guid cartId, Guid productId)
        {
            var cartDetails = await _cartRepo.GetCartDetailByProductId(cartId, productId );
            if (cartDetails == null)
            {
                return NotFound();
            }
            return Ok(cartDetails);
        }



        [HttpPost("Create")]
        public async Task<IActionResult> Create(CartDetail cartDetail)
        {
            await _cartRepo.Create(cartDetail); // Thêm await cho đúng
            return Ok();
        }
        [HttpPut("Update")]
        public async Task<IActionResult> Update( Guid cartId, Guid productId, int quantity)
		{
            await _cartRepo.Update(cartId, productId, quantity);
			return Ok();
        }
        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _cartRepo.Delete(id);
            return Ok();
        }

		[HttpPost("AddToCart")]
		public async Task<IActionResult> AddToCart(Guid cartId, Guid productId, int quantity, Guid sizeId)
		{
			try
			{
				await _cartRepo.AddToCart(cartId, productId, sizeId, quantity );
				return Ok();
			}
			catch (Exception ex)
			{
				return StatusCode(500, new { message = "An error occurred while adding the product to the cart.", details = ex.InnerException?.Message });
			}

		}

        [HttpPost("CheckOut")] 
        public async Task<IActionResult> CheckOut(Guid cartId)
        {
            try
            {
                await _cartRepo.CheckOut(cartId);
                return Ok();
            }
            catch (Exception ex)
            {

				return StatusCode(500, new { message = "An error from checkout.", details = ex.InnerException?.Message });
			}
		}

	}
}
