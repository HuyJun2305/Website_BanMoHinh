using Data.Models;
using Microsoft.AspNetCore.Mvc;
using View.IServices;
using View.Services;
using View.ViewModels;

namespace View.Controllers
{
	public class OrdersController : Controller
	{
        private readonly IOrderServices _orderServices;
        private readonly IOrderDetailServices _orderDetailServices;
        private readonly IProductServices _productServices;
        private readonly IImageServices _imageServices;
        private readonly IUserServices _accountServices;

        public OrdersController(IOrderServices orderServices, IUserServices accountServices, IProductServices productServices, IImageServices imageServices, IOrderDetailServices orderDetailServices)
        {
            _orderServices = orderServices;
            _accountServices = accountServices;
            _productServices = productServices;
            _imageServices = imageServices;
            _orderDetailServices = orderDetailServices;
        }

        public async Task<IActionResult> Index()
        {
            // Lấy staffId từ session
            var staffIdString = HttpContext.Session.GetString("userId");

            if (string.IsNullOrEmpty(staffIdString))
            {
                return BadRequest("Staff ID is missing from session.");
            }

            // Chuyển đổi staffId từ chuỗi sang Guid
            if (!Guid.TryParse(staffIdString, out var staffId))
            {
                return BadRequest("Invalid staffId in session.");
            }

            // Lấy danh sách các trạng thái đơn hàng
            var orderStatuses = Enum.GetValues(typeof(OrderStatus)).Cast<OrderStatus>();

            var ordersByStatus = new Dictionary<OrderStatus, IEnumerable<OrderViewModel>>();

            // Lấy danh sách các đơn hàng cho từng trạng thái và chuyển thành OrderViewModel
            foreach (var status in orderStatuses)
            {
                var orders = await _orderServices.GetOrdersByStatus(status);

                var orderViewModels = orders.Select(o => new OrderViewModel
                {
                    OrderId = o.Id,
                    CustomerName = o.CustomerName,
                    Price = o.Price,
                    PaymentMethods = o.PaymentMethods,
                    PaymentStatus = o.PaymentStatus,
                    DayCreate = o.DayCreate,
                    Status = o.Status,
                    Note = o.Note,
                    PhoneNumber = o.Account.PhoneNumber
                    
                    
                });

                ordersByStatus[status] = orderViewModels;
            }

            // Trả về view với danh sách các đơn hàng theo trạng thái
            var model = new OrderIndexViewModel
            {
                OrdersByStatus = ordersByStatus
            };

            return View(model);
        }





    }
}
