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
		private readonly IUserServices _accountServices;

        public CounterSaleController(IOrderServices orderServices, IOrderDetailServices orderDetailServices,
			IProductServices productServices, IImageServices imageServices, IUserServices accountServices)
        {
            _orderServices = orderServices;
            _orderDetailServices = orderDetailServices;
            _productServices = productServices;
            _imageServices = imageServices;
            _accountServices = accountServices;
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
					var userIdClaim = jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

					if (Guid.TryParse(userIdClaim, out Guid userId))
					{
						return userId; // Trả về userId nếu parse thành công
					}
				}
			}

			return Guid.Empty; // Nếu không lấy được userId, trả về Guid.Empty
		}


        public async Task<IActionResult> Index2()
        {
            // Lấy staffId từ session
            var staffIdString = HttpContext.Session.GetString("userId");
            var products = await _productServices.GetAllProduct();
            var orders = await _orderServices.GetAllOrderStatus0();

            var orderId = orders.FirstOrDefault()?.Id ?? null; // Nếu có đơn hàng thì lấy orderId, nếu không có sẽ là null

            var createBy = await _accountServices.GetUserById(staffIdString);
            // Chuyển đổi staffId từ chuỗi sang Guid
            if (!Guid.TryParse(staffIdString, out var staffId))
            {
                return BadRequest("Invalid staffId in session.");
            }
            // Nếu không có orderId thì trả về trang không có orderDetails
            if (orderId == null)
            {
                // Trả về view không có orderDetail
                var modelWithoutOrderDetail = new CounterSalesViewModel
                {
                    products = products,
                    orders = orders,
                    CreateBy = createBy.UserName,
                    StaffId = createBy.Id,
                    OrderId = orderId,
                    DayCreate = null,
                    orderDetails = null, // Không có orderDetail
                    OrderDetailsId = null
                };

                return View(modelWithoutOrderDetail);
            }
            else
            {
                // Nếu có orderId thì lấy orderDetails, nếu không có thì trả về null
                var orderDetail = await _orderDetailServices.GetOrderDetailsByOrderIdAsync(orderId);

                var orderDetailId = orderDetail?.FirstOrDefault()?.Id;

                // Trả về view có orderDetail nếu có orderId
                var modelWithOrderDetail = new CounterSalesViewModel
                {
                    products = products,
                    orders = orders,
                    CreateBy = createBy.UserName,
                    StaffId = createBy.Id,
                    OrderId = orderId,
                    DayCreate = orders.FirstOrDefault().DayCreate,
                    PaymentMethods = orders.FirstOrDefault()?.PaymentMethods ?? PaymentMethod.Cash,
                    orderDetails = orderDetail,
                    OrderDetailsId = orderDetailId
                };

                return View(modelWithOrderDetail); // Trả về view có orderDetail
            }
        }









    }
}
