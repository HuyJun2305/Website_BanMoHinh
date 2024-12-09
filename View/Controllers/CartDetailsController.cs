using Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using View.IServices;
using View.Services;
using View.ViewModels;

namespace View.Controllers
{
    public class CartDetailsController : Controller
    {
        private readonly IProductServices _productServices;
        private readonly IImageServices _imageServices; 
        private readonly ICartServices _cartServices;

        public CartDetailsController(ICartServices cartServices, IProductServices productServices, IImageServices imageServices)
        {
            _cartServices = cartServices;
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



		public async Task<ActionResult> Index()
		{
			var userId = GetUserIdFromJwtInSession(); // Lấy userId từ JWT trong session

			if (userId != Guid.Empty)
			{
				// Lấy thông tin giỏ hàng của userId
				var cart = await _cartServices.GetCartByUserId(userId);

				if (cart != null)
				{
					var cartDetails = await _cartServices.GetCartDetailByCartId(cart.Id);
					var products = await _productServices.GetAllProduct();
					var selectedImage = await _imageServices.GetAllImages();

					// Nếu có sản phẩm, trả về view với dữ liệu giỏ hàng
					var productData = new CartDetailsViewModel()
					{
						CartId = cart.Id,
						Products = products,
						Images = selectedImage,
						CartDetails = cartDetails
					};

					return View(productData);
				}
			}

			// Nếu không tìm thấy userId hoặc giỏ hàng, chuyển đến trang đăng nhập
			return RedirectToAction("Login", "Authentication");
		}





	}
}
