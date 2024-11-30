using Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using View.IServices;
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

        // GET: CartDetailsController
        public ActionResult Index()
        {
            var products = _productServices.GetAllProduct().Result;
            var selectedImage = _imageServices.GetAllImages().Result;
            var cartDetails = _cartServices.GetAllCartDetails().Result;
            if (products == null) return View("'Product is null'");
            var productData = new CartDetailsViewModel()
            {
                Products = products,
                Images = selectedImage,
                CartDetails = cartDetails
            };
            return View(productData);
        }
        



		public async Task<IActionResult> RemoveFromCart(Guid id)
		{
			var cartDetail = await _cartServices.GetCartDetailById(id);
			if (cartDetail != null)
			{
				await _cartServices.Delete(cartDetail.Id);
				return RedirectToAction("Index", new { cartId = cartDetail.Id });
			}
			return View("Error");
		}
	}
}
