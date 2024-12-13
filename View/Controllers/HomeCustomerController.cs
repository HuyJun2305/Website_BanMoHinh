using Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using View.IServices;
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
			var userId = GetUserIdFromJwtInSession();

			var cart = await _cartServices.GetCartByUserId(userId);
			if (cart == null)
			{
				return NotFound("Cart not found for the user.");
			}
			var cartId = cart.Id;

			// Lấy thông tin sản phẩm
			var products = await _productServices.GetAllProduct();
			var selectedProduct = products.FirstOrDefault(p => p.Id == id);
			if (selectedProduct == null)
			{
				return NotFound("Product not found!");
			}

			// Lấy danh sách kích thước và stock
			var allProductSizes = await _productServices.GetAllProductSizes();
			var productSizes = allProductSizes.Where(ps => ps.ProductId == id).ToList();

			var allSizes = await _sizeServices.GetAllSizes(); // Lấy tất cả kích thước để khớp tên
			var sizes = productSizes.Select(ps => new ProductSizeViewModel
			{
				SizeId = ps.SizeId,
				Value = allSizes.FirstOrDefault(s => s.Id == ps.SizeId)?.Value, // Lấy tên kích thước từ danh sách kích thước
				Stock = ps.Stock,
				Selected = false // Hoặc thiết lập logic để đánh dấu kích thước mặc định
			}).ToList();

			// Đánh dấu kích thước mặc định nếu cần
			if (sizes.Any())
			{
				sizes[0].Selected = true; // Chọn kích thước đầu tiên làm mặc định
			}

			// Lọc hình ảnh liên quan đến sản phẩm
			var relatedImages = await _imageServices.GetAllImages();
			var productDetailData = new ProductIndexVM
			{
				Products = new List<Product> { selectedProduct },
				Images = relatedImages.Where(i => i.ProductId == id).ToList(),
				CartId = cartId,
				Sizes = sizes // Gán danh sách kích thước vào model
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
