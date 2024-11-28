using Microsoft.AspNetCore.Mvc;
using View.IServices;
using View.ViewModels;

namespace View.Controllers
{
    public class HomeCustomerController : Controller
    {
        private readonly IProductServices _productServer;
        private readonly ISizeServices _sizeServices;
        private readonly IBrandServices _brandServices;
        private readonly IMaterialServices _materialServices;
        private readonly IImageServices _imageServices;
        public HomeCustomerController(IProductServices productServices, ISizeServices sizeServices, IBrandServices brandServices, IMaterialServices materialServices, IImageServices imageServices)
        {
            _productServer = productServices;
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
            var products = _productServer.GetAllProduct().Result;
            var selectedImage = _imageServices.GetAllImages().Result;
            if (products == null) return View("'Product is null'");
            var productData = new ProductIndex()
            {
                Products = products,
                Images = selectedImage,
            };
            return View(productData);
        }
    }
}
