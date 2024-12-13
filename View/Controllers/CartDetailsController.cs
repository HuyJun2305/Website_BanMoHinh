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
        private readonly ISizeServices _sizeServices;
        public CartDetailsController(ICartServices cartServices, IProductServices productServices, IImageServices imageServices, ISizeServices sizeServices )
        {
            _cartServices = cartServices;
            _productServices = productServices;
            _imageServices = imageServices;
            _sizeServices = sizeServices;
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




        public async Task<IActionResult> Index()
        {
            var userId = GetUserIdFromJwtInSession(); // Lấy userId từ JWT trong session

            if (userId != Guid.Empty)
            {
                // Lấy giỏ hàng của người dùng
                var cart = await _cartServices.GetCartByUserId(userId);
                if (cart != null)
                {
                    // Lấy chi tiết giỏ hàng theo CartId
                    var cartDetails = await _cartServices.GetCartDetailByCartId(cart.Id);
                    var products = await _productServices.GetAllProduct(); // Lấy tất cả sản phẩm
                    var images = await _imageServices.GetAllImages(); // Lấy tất cả hình ảnh
                    var productSizes = await _productServices.GetAllProductSizes(); // Lấy tất cả mối quan hệ sản phẩm-kích thước
                    // Tạo danh sách chi tiết giỏ hàng (CartDetailViewModel)
                    var cartDetailViewModels = new List<CartDetailVM>();
                    
                    // Lặp qua các CartDetail trong giỏ hàng
                    foreach (var cartDetail in cartDetails)
                    {
                        var product = products.FirstOrDefault(p => p.Id == cartDetail.ProductId);

                        if (product != null)
                        {
                            //// Lấy tên kích thước từ CartDetail (SizeId có trong CartDetail)
                            var sizeName = productSizes.FirstOrDefault(ps => ps.SizeId == cartDetail.SizeId)?.Size.Value ?? "N/A";
                            var sizeStock = productSizes.FirstOrDefault(ps => ps.SizeId == cartDetail.SizeId).Stock;
                            var sizeId = productSizes.FirstOrDefault(p => p.SizeId == cartDetail.SizeId).SizeId;
                            var weight = productSizes.FirstOrDefault(ps => ps.SizeId == cartDetail.SizeId).Size.Weight;
                            // Lấy hình ảnh của sản phẩm
                            var productImages = images.Where(i => i.ProductId == cartDetail.ProductId).ToList();
                            
                            // Tạo CartDetailVM cho từng bản ghi CartDetail
                            var cartDetailVM = new CartDetailVM
                            {
                                Id = cartDetail.Id, // Đảm bảo giữ đúng CartDetail.Id từ cơ sở dữ liệu
                                Quantity = cartDetail.Quatity,
                                TotalPrice = cartDetail.TotalPrice,
                                CartId = cartDetail.CartId,
                                ProductName = product.Name,
                                SizeName = sizeName, // Gán tên kích thước cho CartDetail
                                Price = product.Price,
                                ProductId = product.Id,
                                SizeId = sizeId,
                                Stock = sizeStock,
                                Weight = weight
                            };

                            // Thêm CartDetailVM vào danh sách
                            cartDetailViewModels.Add(cartDetailVM);
                        }
                    }

                    // Tạo ViewModel cho giỏ hàng
                    var cartDetailViewModel = new CartDetailViewModel
                    {
                        CartDetails = cartDetailViewModels,
                        Images = images // Bao gồm các hình ảnh sản phẩm    
                    };

                    return View(cartDetailViewModel);
                }
            }

            return RedirectToAction("Login", "Authentication");
        }







    }
}
