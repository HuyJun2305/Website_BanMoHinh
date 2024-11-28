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

        // GET: CartDetailsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CartDetailsController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CartDetailsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CartDetailsController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CartDetailsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CartDetailsController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CartDetailsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
