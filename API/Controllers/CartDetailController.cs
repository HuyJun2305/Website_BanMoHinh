using API.IRepositories;
using Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartDetailController : ControllerBase
    {
        private readonly ICartDetailRepo _cartdetailRepo;

        public CartDetailController(ICartDetailRepo cartdetailRepo)
        {
            _cartdetailRepo = cartdetailRepo;
        }
        [HttpGet("GetAllCartDetails")]
        public async Task<IActionResult> GetAllCartDetails()
        {
            var lstCartDetails = await _cartdetailRepo.GetAllCartDetail();
            return Ok(lstCartDetails);
        }
        [HttpGet("GetCartDetailsByCartId")]
        public async Task<IActionResult> GetCartDetailByCartId(Guid cartId)
        {
            var listCartDetails = await _cartdetailRepo.GetCartDetailById(cartId);
            if (listCartDetails == null  )
            {
                return NotFound();
            }
            return Ok(listCartDetails);
        }
        [HttpGet("GetCartDetailsById")]
        public async Task<IActionResult> GetCartDetailById(Guid id)
        {
            var cartDetails = await _cartdetailRepo.GetCartDetailById(id);
            if (cartDetails == null)
            {
                return NotFound();
            }
            return Ok(cartDetails);
        }
        [HttpPost("Create")]
        public async Task<IActionResult> Create(CartDetail cartDetail)
        {
            await _cartdetailRepo.Create(cartDetail); // Thêm await cho đúng
            return Ok();
        }
        [HttpPut("Update")]
        public async Task<IActionResult> Update(CartDetail cartDetails, Guid id)
        {
            await _cartdetailRepo.Update(cartDetails, id);
            return Ok();
        }
        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _cartdetailRepo.Delete(id);
            return Ok();
        }

    }
}
