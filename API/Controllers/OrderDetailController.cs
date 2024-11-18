using API.IRepositories;
using Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderDetailController : ControllerBase
    {
        private readonly IOrderDetailRepo _orderDetailRepo;
        public OrderDetailController(IOrderDetailRepo orderDetailRepo)
        {
            _orderDetailRepo = orderDetailRepo;
        }
        // GET: api/<OrderDetailsController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDetail>>> Get()
        {
            return await _orderDetailRepo.GetAllOrderDetail();
        }

        // GET api/<OrderDetailsController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDetail>> Get(Guid id)
        {
            return await _orderDetailRepo.GetOrderdetailById(id);
        }

        [HttpGet("OrderId/{id}")]
        public async Task<ActionResult<IEnumerable<OrderDetail>>> GetOrderDetailsByOrderId(Guid id)
        {
            return await _orderDetailRepo.GetOrderDetailsByOrderId(id);
        }

        // POST api/<OrderDetailsController>
        [HttpPost]
        public async Task<ActionResult<OrderDetail>> Post(OrderDetail orderDetail)
        {
            try
            {
                await _orderDetailRepo.Create(orderDetail);
                await _orderDetailRepo.SaveChanges();
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }

            return CreatedAtAction("Get", new { id = orderDetail.Id }, orderDetail);
        }

        // PUT api/<OrderDetailsController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, OrderDetail orderDetail)
        {
            try
            {
                await _orderDetailRepo.Update(orderDetail);
                await _orderDetailRepo.SaveChanges();
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }

            return CreatedAtAction("Get", new { id = orderDetail.Id }, orderDetail);
        }

        // DELETE api/<OrderDetailsController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _orderDetailRepo.Delete(id);
                await _orderDetailRepo.SaveChanges();
            }
            catch (Exception ex)
            {
                Problem(ex.Message);
            }

            return Content("Success");
        }
    }
}
