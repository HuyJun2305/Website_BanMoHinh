using API.IRepositories;
using API.Repositories;
using Data.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{

	[Route("api/[controller]")]
	[ApiController]
	public class OrderDetailsController : ControllerBase
	{
		private readonly IOrderDetailRepo _orderDetailRepo;
		private readonly IOrderRepo _orderRepo;
		private readonly IProductRepos _productRepo;

		public OrderDetailsController(IOrderDetailRepo orderDetailRepo, IOrderRepo orderRepo, IProductRepos productRepo)
		{
			_orderDetailRepo = orderDetailRepo;
			_orderRepo = orderRepo;
			_productRepo = productRepo;
		}

		[HttpGet("GetOrderDetailByOrderId")]
		public async Task<ActionResult<List<OrderDetail>>> GetOrderDetailByOrderId(Guid? orderId)
		{
			try
			{
				var getOrder = await _orderDetailRepo.GetOrderDetailsByOrderIdAsync(orderId);
				if (getOrder == null)
				{
					return NotFound();
				}
				return Ok(getOrder);

			}
			catch (Exception ex)
			{
				return Problem(ex.Message);
			}
		}
		[HttpPost("AddOrUpdateOrderDetail")]
		public async Task<IActionResult> AddOrUpdateOrderDetail(Guid orderId, Guid productId, int quantity)
		{
			try
			{
                var orderDetail =  await _orderDetailRepo.AddOrUpdateOrderDetail(orderId, productId, quantity);
				return Ok(orderDetail);
			}
			catch (KeyNotFoundException ex)
			{
				return NotFound(ex.Message);
			}
			catch (Exception ex)
			{
				return Problem(ex.Message);
			}
		}
		[HttpPut("UpdateOrderDetail")]
		public async Task<IActionResult> UpdateOrderDetail(Guid orderId, Guid productId, int quantity)
		{
            // Cập nhật order detail logic
            var orderDetail = await _orderDetailRepo.UpdateOrderDetail(orderId, productId, quantity);

            if (orderDetail != null)
            {
                return Ok(orderDetail); // Trả về JSON
            }
            else
            {
                return NoContent(); // Trả về 204 nếu không có nội dung
            }
        }

		[HttpPost("RemoveOrderDetail")]
		public async Task<ActionResult> RemoveOrderDetail(Guid orderId, Guid productId, int quantity)
		{
			try
			{
				var result = await _orderDetailRepo.RemoveOrderDetail(orderId, productId, quantity);
				return Ok(result); 
			}
			catch (KeyNotFoundException ex)
			{
				return Problem(ex.Message); 
			}
			
		}

        
    }
}

