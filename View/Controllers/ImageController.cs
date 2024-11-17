using Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using View.IServices;
using View.Servicecs;

namespace View.Controllers
{
    public class ImageController : Controller
    {
        private readonly IImageServices _imageServices;
        private readonly IProductServices _productServices;
        public ImageController(IImageServices imageServices, IProductServices productServices)
        {
            _imageServices = imageServices;
            _productServices = productServices;
        }
        public async Task<IActionResult> Index()
        {
            var products = _productServices.GetAllProduct().Result;
            if (products == null) return View("'Product is null!'");
            var viewContext = await _imageServices.GetAllImages();
            if (viewContext == null || !viewContext.Any())
            {
                ViewBag.Message = "No product details found.";
                return View(new List<Image>());
            }
            return View(viewContext);
        }
    }
}
