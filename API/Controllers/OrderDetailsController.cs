using API.IRepositories;
using API.Repositories;
using Data.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlTypes;

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
		public async Task<IActionResult> AddOrUpdateOrderDetail(Guid orderId, Guid productId, Guid sizeId, int quantity)
		{
			try
			{
                await _orderDetailRepo.AddOrUpdateOrderDetail(orderId, productId, sizeId, quantity);
				return Ok(new {success = true, message ="Successfully"});
			}

			catch (Exception ex)
			{
                return Problem(detail: ex.Message, title: "Add to order failed");
            }
		}
		[HttpPut("UpdateOrderDetail")]
		public async Task<IActionResult> UpdateOrderDetail(Guid orderId, Guid productId,Guid sizeId, int quantity)
		{
			try
			{
                await _orderDetailRepo.UpdateOrderDetail(orderId, productId, sizeId, quantity);
                return Ok(new { success = true, message = "Update success" });

            }
            catch (Exception ex)
			{
                return Problem(detail: ex.Message, title: "update failed");
            }
        }

		[HttpPost("RemoveOrderDetail")]
		public async Task<ActionResult> RemoveOrderDetail(Guid orderId, Guid productId, Guid sizeId)
		{
			try
			{
				await _orderDetailRepo.RemoveOrderDetail(orderId, productId, sizeId);
				return Ok(new { success = true, message ="Remove success" }); 
			}
			catch (KeyNotFoundException ex)
			{
				return Problem(detail: ex.Message, title: "remove failed"); 
			}
			
		}

        
    }
}

