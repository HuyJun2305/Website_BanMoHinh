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
                    return Problem(detail: ex.Message, title: "Create failed");
            }
        }
		[HttpPost("CheckOutInStore")]
		public async Task<ActionResult> CheckOutInStore([FromQuery] Guid orderId, [FromQuery] Guid staffId, [FromQuery]  decimal amountGiven,  PaymentMethod paymentMethod)
		{
            try
            {
                await _orderRepo.CheckOutInStore(orderId, staffId, amountGiven, paymentMethod);
                return Ok(new { success = true, message = "Checkout in store successfully" });

            }
            catch (Exception ex)
            {

                return Problem(detail: ex.Message, title: "Checkout in store failed");
            }

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
                return Ok(new { success = true, message = "Order accepted successfully" });
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message, title: "Order Acceptance Failed");
            }
        }
		[HttpPost("CancelOrder")]
		public async Task<ActionResult> CancelOrder(Guid orderId, string? note)
		{
			try
			{
                await _orderRepo.CancelOrder(orderId, note);
                return Ok(new { success = true, message = "Order canceled" });

            }
            catch (Exception ex)
			{

                return Problem(detail: ex.Message, title: "Order Cancel Failed");
            }
        }
		[HttpPost("DeliveryOrder")]
		public async Task<ActionResult> DeliveryOrder(Guid orderId, string? note)
		{
			try
			{
                await _orderRepo.DeliveryOrder(orderId, note);
                return Ok(new {success = true, message = "Order deliveried susscesfully" });

            }
            catch (Exception ex)
			{
                return Problem(detail: ex.Message, title: "Order Delivery Failed");
            }
        }
		[HttpPost("ComplateOrder")]
		public async Task<ActionResult> ConplateOrder(Guid orderId, string? note)	
		{
			try
			{
                await _orderRepo.ConplateOrder(orderId, note);
                return Ok(new { success = true, message = "Order completed successfully" });
            }
            catch (Exception ex)
			{
                return Problem(detail: ex.Message, title: "Order Complate Failed");
            }
        }
        [HttpPost("PaidOrder")]
		public async Task<ActionResult> PaidOrder(Guid orderId, string? note)	
		{
			try
			{
                await _orderRepo.PaidOrder(orderId, note);
                return Ok(new { success = true, message = "Order paid successfully" });
            }
            catch (Exception ex)
			{
                return Problem(detail: ex.Message, title: "Order Paid Failed");
            }
        }
		[HttpPost("RefundOrder")]
		public async Task<ActionResult> RefundOrder(Guid orderId, string note)
		{
            try
            {
                await _orderRepo.RefundByCustomer(orderId, note);
                return Ok(new { success = true, message = "Refund order waiting admin " });

            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message, title: "Order Refund Failed");
            }
        }
		[HttpPost("ShippingError")]
		public async Task<ActionResult> ShippingError(Guid orderId, string? note)
		{
            try
            {
                await _orderRepo.ShippingError(orderId, note);
                return Ok(new {success =true, message = "Shipper on waiting" });

            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message, title: "Order Shipping Failed");
            }
        }
		[HttpPost("MissingInformation")]
		public async Task<ActionResult> MissingInformation(Guid orderId, string? note)
		{
            try
            {
                await _orderRepo.MissingInformation(orderId, note);
                return Ok(new { success = true, message = "On waiting to new Infomation" });

            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message, title: "Order MissingInformation Failed");
            }
        }
		[HttpPost("LostOrder")]
		public async Task<ActionResult> LoseOrder(Guid orderId, string? note)
		{
            try
            {
                await _orderRepo.LoseOrder(orderId, note);
                return Ok(new {success =true, message = "Lost order" });

            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message, title: "Order Complate Failed");
            }
        }
        [HttpPost("Accident")]
		public async Task<ActionResult> Accident(Guid orderId, string? note)
		{
            try
            {
                await _orderRepo.Accident(orderId, note);
                return Ok(new {success =true, message = "Lost order" });

            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message, title: "Order Complate Failed");
            }
        }
        [HttpPost("AcceptRefund")]
		public async Task<ActionResult> AcceptRefund(Guid orderId, string? note)
		{
            try
            {
                await _orderRepo.AcceptRefund(orderId, note);
                return Ok(new {success =true, message = "Refund Order success" });

            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message, title: "Refund Order Failed");
            }
        }
        [HttpPost("CancelRefund")]
		public async Task<ActionResult> CancelRefund(Guid orderId, string? note)
		{
            try
            {
                await _orderRepo.CancelRefund(orderId, note);
                return Ok(new {success =true, message = "Cancel refund order success" });

            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message, title: "Cancel refund order failed\"");
            }
        }
        [HttpPost("ReOrder")]
		public async Task<ActionResult> ReOrder(Guid orderId, string? note)
		{
            try
            {
                await _orderRepo.ReOrder(orderId, note);
                return Ok(new {success =true, message = "Reorder order success" });

            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message, title: "Cancel refund order failed\"");
            }
        }
        [HttpPost("ReShip")]
		public async Task<ActionResult> ReShip(Guid orderId, string? note)
		{
            try
            {
                await _orderRepo.ReShip(orderId, note);
                return Ok(new {success =true, message = "Cancel refund order success" });

            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message, title: "Cancel refund order failed\"");
            }
        }
        [HttpGet("GetOrderDetails")]
        public async Task<ActionResult> GetOrderDetails(Guid orderId)
        {
            try
            {
                var order = await _orderRepo.GetOrderDetails(orderId);
                return Ok(order);
            }   
            catch (Exception ex)
            {

                return Problem(detail: ex.Message, title: "failed");
            }
        }
    }
}
