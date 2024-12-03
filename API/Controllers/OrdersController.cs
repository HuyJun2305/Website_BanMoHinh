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

		[HttpGet("GetAllOrder")]
		public async Task<ActionResult<List<Order>>> GetAllOrder()
		{
			try
			{
				return await _orderRepo.GetAllOrder();

			}
			catch (Exception ex)
			{
				return Problem(ex.Message);
			}
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

		


	}
}
