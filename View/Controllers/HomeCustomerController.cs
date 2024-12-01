using Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;
using View.IServices;
using View.Services;
using View.Utilities.Extensions;
using View.ViewModels;

namespace View.Controllers
{
    public class HomeCustomerController : Controller
    {
        private readonly IProductServices _productServices;
        private readonly ISizeServices _sizeServices;
        private readonly IBrandServices _brandServices;
        private readonly IMaterialServices _materialServices;
        private readonly IImageServices _imageServices;
		private readonly IAuthenticationService _authenticationService;
		private readonly ICartServices _cartServices;
        public HomeCustomerController(IProductServices productServices, ISizeServices sizeServices,
			IBrandServices brandServices, IMaterialServices materialServices, IImageServices imageServices, ICartServices cartServices)
		{
			_productServices = productServices;
			_sizeServices = sizeServices;
			_brandServices = brandServices;
			_materialServices = materialServices;
			_imageServices = imageServices;
			_cartServices = cartServices;
		}
		public IActionResult Index()
        {
			var username = HttpContext.Session.GetString("username");
			ViewBag.Username = username;

			return View();
        }


        public IActionResult ViewProducts()
        {
            var products = _productServices.GetAllProduct().Result;
            var selectedImage = _imageServices.GetAllImages().Result;
            if (products == null) return View("'Product is null'");
            var productData = new ProductIndex()
            {
                Products = products,
                Images = selectedImage,
            };
            return View(productData);
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

		public async Task<IActionResult> ViewProductDetails(Guid id)
		{
			// Lấy userId từ JWT trong session
			var userId = GetUserIdFromJwtInSession(); // Hàm lấy userId từ JWT đã lưu trong session


			// Lấy giỏ hàng của người dùng dựa trên userId
			var cart = await _cartServices.GetCartByUserId(userId);

			if (cart == null)
			{
				// Nếu không tìm thấy giỏ hàng, có thể tạo mới giỏ hàng hoặc trả về lỗi
				return NotFound("Cart not found for the user.");
			}

			// Lấy cartId từ giỏ hàng
			var cartId = cart.Id;

			// Lấy thông tin sản phẩm và hình ảnh
			ViewData["SizeId"] = new SelectList(_sizeServices.GetAllSizes().Result, "Id", "Value");
			var products = _productServices.GetAllProduct().Result;
			var selectedImage = _imageServices.GetAllImages().Result;
			var selectedProduct = products.FirstOrDefault(p => p.Id == id);

			if (selectedProduct == null)
			{
				return NotFound("Product not found!");
			}

			var relatedImages = selectedImage.Where(i => i.ProductId == id).ToList();
			var relatedProductDetails = products.Where(d => d.Id == id).ToList();

			// Tạo CartDetailsViewModel và thêm cartId vào
			var productDetailData = new ProductIndex
			{
				Products = new List<Product> { selectedProduct },
				Images = relatedImages,
				CartId = cartId // Thêm cartId vào ViewModel
			};

			return View(productDetailData);
		}








		private Guid GetUserId()
		{
			if (HttpContext.User.Identity.IsAuthenticated)
			{
				var userIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "sub" || c.Type == "id");
				if (userIdClaim != null && Guid.TryParse(userIdClaim.Value, out var userId))
				{
					return userId;
				}
			}

			return Guid.Empty; // Guest
		}


	}




}
