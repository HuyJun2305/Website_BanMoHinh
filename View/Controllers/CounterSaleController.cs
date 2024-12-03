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

		public ActionResult Index()
		{

			var userId = GetUserIdFromJwtInSession(); // Lấy userId từ JWT trong session
			var orderId = _orderServices.GetOrderById(userId).Result;	

			if (userId != Guid.Empty)
			{
				var orders =  _orderServices.GetAllOrder().Result;
				var orderDetails = _orderDetailServices.GetOrderDetailsByOrderIdAsync(orderId.Id).Result;
				var products = _productServices.GetAllProduct().Result;

				var counterSaleDate = new CounterSalesViewModel
				{
					orders = orders,
					products = products,
					orderDetails = orderDetails

				};

				return View(counterSaleDate);
			}

			// Nếu không tìm thấy userId hoặc giỏ hàng, chuyển đến trang đăng nhập
			return RedirectToAction("Login", "Authentication");
		}


	}
}
