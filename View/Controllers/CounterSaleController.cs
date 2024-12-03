using Data.Models;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using View.IServices;
using View.Services;
using View.ViewModels;

namespace View.Controllers
{
	public class CounterSaleController : Controller
	{
		private readonly IOrderServices _orderServices;
		private readonly IOrderDetailServices _orderDetailServices;
		private readonly IProductServices _productServices;
		private readonly IImageServices	_imageServices;

        public CounterSaleController(IOrderServices orderServices, IOrderDetailServices orderDetailServices, IProductServices productServices, IImageServices imageServices)
        {
            _orderServices = orderServices;
            _orderDetailServices = orderDetailServices;
            _productServices = productServices;
            _imageServices = imageServices;
        }

        private Guid GetUserIdFromJwtInSession()
		{
			var jwtToken = HttpContext.Session.GetString("jwtToken"); // Lấy JWT từ session

			if (!string.IsNullOrEmpty(jwtToken))
			{
				var tokenHandler = new JwtSecurityTokenHandler();

				// Kiểm tra xem token có hợp lệ không
				if (tokenHandler.CanReadToken(jwtToken))
				{
					var jwt = tokenHandler.ReadJwtToken(jwtToken);

					// Lấy claim chứa userId từ token
					var userIdClaim = jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

					if (Guid.TryParse(userIdClaim, out Guid userId))
					{
						return userId; // Trả về userId nếu parse thành công
					}
				}
			}

			return Guid.Empty; // Nếu không lấy được userId, trả về Guid.Empty
		}

        //public async Task<IActionResult> Index()
        //{
        //    var userId = GetUserIdFromJwtInSession(); 

        //    var order = await _orderServices.GetOrderById(userId); 

        //    if (order == null)
        //    {
        //        return View(); 
        //    }

        //    // Lấy chi tiết đơn hàng nếu có
        //    var orderDetails = await _orderDetailServices.GetOrderDetailsByOrderIdAsync(order.Id); 
        //    var products = await _productServices.GetAllProduct(); 

        //    var counterSaleDate = new CounterSalesViewModel
        //    {
        //        orders = await _orderServices.GetAllOrder(), 
        //        products = products,
        //        orderDetails = orderDetails
        //    };

        //    return View(counterSaleDate); 
        //}
        public async Task<IActionResult> Index()
        {
            // Lấy tất cả đơn hàng từ cơ sở dữ liệu
            var orders = await _orderServices.GetAllOrder();

            Order order;

            // Kiểm tra nếu danh sách đơn hàng trống
            if (orders == null || !orders.Any())
            {
                // Nếu không có đơn hàng, tạo một đơn hàng mới
                order = new Order
                {
                    Id = Guid.NewGuid(),
                    AccountId = GetUserIdFromJwtInSession(), // Gán userId cho đơn hàng
                    DayCreate = DateTime.UtcNow,
                    CreateBy = GetUserIdFromJwtInSession(), // Gán userId cho đơn hàng
                    Status = OrderStatus.TaoDonHang // Hoặc trạng thái mặc định khác
                };

                // Lưu đơn hàng mới vào cơ sở dữ liệu
                await _orderServices.Create(order);
            }
            else
            {
                // Nếu có ít nhất một đơn hàng, lấy đơn hàng đầu tiên
                order = orders.FirstOrDefault();
            }

            // Lấy chi tiết đơn hàng
            var orderDetails = await _orderDetailServices.GetOrderDetailsByOrderIdAsync(order.Id);

            // Lấy tất cả sản phẩm để hiển thị
            var products = await _productServices.GetAllProduct();

            // Tạo và trả về view model cho view
            var counterSaleDate = new CounterSalesViewModel
            {
                OrderId = order.Id,
                //orders = order,
                products = products,
                orderDetails = orderDetails
            };

            return View(counterSaleDate);
        }








    }
}
