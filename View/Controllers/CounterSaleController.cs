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
        public async Task<IActionResult> Index2()
        {
            var products = await _productServices.GetAllProduct();

            // Trả về view với dữ liệu ban đầu
            var model = new CounterSalesViewModel
            {
                products = products // Hiển thị danh sách sản phẩm ban đầu
            };

            return View(model);
        }








    }
}
