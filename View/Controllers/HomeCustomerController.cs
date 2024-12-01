using Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        public HomeCustomerController(IProductServices productServices, ISizeServices sizeServices, IBrandServices brandServices, IMaterialServices materialServices, IImageServices imageServices)
        {
            _productServices = productServices;
            _sizeServices = sizeServices;
            _brandServices = brandServices;
            _materialServices = materialServices;
            _imageServices = imageServices;

        }
        public IActionResult Index()
        {
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

		public async Task<IActionResult> ViewProductDetails(Guid id)
		{
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

			var productDetailData = new ProductIndex
			{
				Products = new List<Product> { selectedProduct },
				Images = relatedImages
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
