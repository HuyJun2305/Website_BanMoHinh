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
        public async Task<IActionResult> Update( Guid cartDetailId, Guid productId, Guid sizeId, int quantity)
		{
            try
            {
                await _cartRepo.Update(cartDetailId, productId, sizeId, quantity);
                return Ok(new { success = true, message = "Cập nhật thành công!" });
            }
            catch (Exception)
            {
                return StatusCode(500, new { success = false, message = "Đã có lỗi xảy ra. Vui lòng thử lại!" });
            }
        }
        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _cartRepo.Delete(id);
            return Ok("Cart updated successfully.");
        }

		[HttpPost("AddToCart")]
		public async Task<IActionResult> AddToCart(Guid cartId, Guid productId, int quantity, Guid sizeId)
		{
			try
			{
				await _cartRepo.AddToCart(cartId, productId, sizeId, quantity );
				return Ok("The product has been added to the cart");
			}
			catch (Exception ex)
			{
				return StatusCode(500, new { message = "An error occurred while adding the product to the cart.", details = ex.InnerException?.Message });
			}

		}

        [HttpPost("CheckOut")]
        public async Task<IActionResult> CheckOut([FromBody] List<Guid> cartDetailIds, decimal
            shippingFee, string city, string district, string ward, string addressDetail)
        {
            try
            {
                // Gọi phương thức xử lý checkout từ repository (repo)
                await _cartRepo.CheckOut(cartDetailIds, shippingFee, city,district,ward,addressDetail);
                return Ok(new { success = true, message = "Checkout successful!" });
            }
            catch (Exception ex)
            {
                // Trả về lỗi 500 nếu có sự cố trong quá trình xử lý
                return StatusCode(500, new { success = false, message = "An error occurred during checkout.", details = ex.InnerException?.Message });
            }
            }


    }
}
