using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Data;
using Data.Models;
using API.IRepositories;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderRepo _orderRepo;

        public OrdersController(IOrderRepo orderRepo)
        {
            _orderRepo = orderRepo;

		}
		[HttpPost]
		public async Task<IActionResult> Create(Order order)
		{
			await _orderRepo.Create(order);
			return Ok();

		}
		[HttpPost("create-by-staff")]
		public async Task<IActionResult> CreateByStaff([FromQuery] Guid staffId, [FromQuery] Guid? customerId = null , [FromQuery] Guid? voucherId = null)
		{
			try
			{
				var newOrder = await _orderRepo.CreateByStaff(staffId, customerId, voucherId);
				return Ok(newOrder);
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}
		[HttpPost("CheckOutInStore")]
		public async Task<ActionResult> CheckOutInStore([FromQuery] Guid orderId, [FromQuery] Guid staffId, [FromQuery]  decimal amountGiven,  PaymentMethod paymentMethod)
		{
				var result =  _orderRepo.CheckOutInStore(orderId, staffId, amountGiven, paymentMethod);
				return Ok(result);
			
		}
		[HttpGet("GetOrderStatus0")]
        public async Task<ActionResult<List<Order>>> GetOrderStatus0()
		{
            var result = await _orderRepo.GetOrderStatus();
            return Ok(result);

        }
        [HttpGet("GetOrdersByStatus")]
        public async Task<ActionResult<List<Order>>> GetOrdersByStatus(OrderStatus status)
        {
            var orders = await _orderRepo.GetOrderByStatus(status);
            return Ok(orders);
        }

        [HttpGet("GetAllOrder")]
		public async Task<ActionResult<List<Order>>> GetAllOrder()
		{
            var result = await _orderRepo.GetAllOrder();
            return Ok(result);
        }

		[HttpGet("GetOrderById")]
		public async Task<ActionResult> GetOrderById(Guid id)
		{
			try
			{
				var order = await _orderRepo.GetOrderById(id);
				return Ok(order);
			}
			catch (Exception ex)
			{

				return Problem(ex.Message);
			}
		}

        [HttpGet("GetOrderByCustomerId")]
        public async Task<ActionResult> GetOrderByCustomerId(Guid customerId)
        {
            try
            {
                var order = await _orderRepo.GetOrderByCustomerId(customerId);
                return Ok(order);
            }
            catch (Exception ex)
            {

                return Problem(ex.Message);
            }
        }
        [HttpGet("GetOrderByCustomerIdAndStatus")]
        public async Task<ActionResult> GetOrdersByCustomerIdAndStatus(Guid customerId, OrderStatus status)
		{
			var order = await _orderRepo.GetOrdersByCustomerIdAndStatus(customerId, status);
			return Ok(order);
		}

        [HttpDelete("DeleteOrderById")]
		public async Task<ActionResult> DeleteOrder(Guid id)
		{
			try
			{
				await _orderRepo.Delete(id);
				return Ok();
			}
			catch (Exception ex)
			{

				return Problem(ex.Message);

			}
		}

		[HttpPost("AcceptOrder")]
        public async Task<ActionResult> AcceptOrder(Guid orderId)
        {
            try
            {
                await _orderRepo.AcceptOrder(orderId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

		[HttpPost("CancelOrder")]
		public async Task<ActionResult> CancelOrder(Guid orderId, string? note)
		{
			await _orderRepo.CancelOrder(orderId, note);
			return Ok();
		}
		[HttpPost("DeliveryOrder")]
		public async Task<ActionResult> DeliveryOrder(Guid orderId, string? note)
		{
			await _orderRepo.DeliveryOrder(orderId, note);
			return Ok();
		}

		[HttpPost("ConplateOrder")]
		public async Task<ActionResult> ConplateOrder(Guid orderId, string? note)	
		{
			await _orderRepo.ConplateOrder(orderId, note);
			return Ok();
		}

		[HttpPost("RefundOrder")]
		public async Task<ActionResult> RefundOrder(Guid orderId, string? note)
		{
			await _orderRepo.RefundOrder(orderId, note);
			return Ok();
		}
		[HttpPost("ShippingError")]
		public async Task<ActionResult> ShippingError(Guid orderId, string? note)
		{
			await _orderRepo.ShippingError(orderId, note);
			return Ok();
		}
		[HttpPost("MissingInformation")]
		public async Task<ActionResult> MissingInformation(Guid orderId, string? note)
		{
			await _orderRepo.MissingInformation(orderId, note);
			return Ok();
		}
		[HttpPost("LoseOrder")]
		public async Task<ActionResult> LoseOrder(Guid orderId, string? note)
		{
			await _orderRepo.LoseOrder(orderId, note);
			return Ok();
		}



    }
}
