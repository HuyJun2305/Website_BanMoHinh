using Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using View.IServices;
using View.ViewModel;

namespace View.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductServices _productServer;
        private readonly ISizeServices _sizeServices;
        private readonly IBrandServices _brandServices;
        private readonly IMaterialServices _materialServices;
        private readonly IImageServices _imageServices;
        public ProductsController(IProductServices productServices, ISizeServices sizeServices, IBrandServices brandServices, IMaterialServices materialServices, IImageServices imageServices)
        {
            _productServer = productServices;
            _sizeServices = sizeServices;
            _brandServices = brandServices;
            _materialServices = materialServices;
            _imageServices = imageServices;
        }
        //
        public async Task<IActionResult> Index()
        {
            var products = _productServer.GetAllProduct().Result;
            var selectedImage = _imageServices.GetAllImages().Result;
            if (products == null) return View("'Product is null'");
            var productData = new ProductIndex()
            {
                Products = products,
                imageSPs = selectedImage,
            };
            return View(productData);
        }
        //
        public async Task<IActionResult> Details(Guid id)
        {
            var product = _productServer.GetProductById(id).Result;
            return View(product);
        }
        //
        public IActionResult Create()
        {
            ViewData["IdSize"] = new SelectList(_sizeServices.GetAllSizes().Result, "Id", "Weight", "Height", "Width");
            ViewData["IdBrand"] = new SelectList(_brandServices.GetAllBrands().Result, "Id", "Name");
            ViewData["IdMaterial"] = new SelectList(_materialServices.GetAllMaterials().Result, "Id", "Name");
            var dataImage = new ProductViewModel()
            {
                Images = _imageServices.GetAllImages().Result
            };
            return View(dataImage);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Price, Name, Description, IdBrand, IdSize, IdMaterial")] Product product)
        {
            if(product.Id != null)
            {
                product.Id = Guid.NewGuid();
                await _productServer.Create(product);
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }
        //
        public async Task<IActionResult> Edit(Guid id)
        {
            if (_productServer.GetAllProduct() == null)
            {
                return NotFound();
            }

            var product = await _productServer.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["IdSize"] = new SelectList(_sizeServices.GetAllSizes().Result, "Id", "Weight", "Height", "Width");
            ViewData["IdBrand"] = new SelectList(_brandServices.GetAllBrands().Result, "Id", "Name");
            ViewData["IdMaterial"] = new SelectList(_materialServices.GetAllMaterials().Result, "Id", "Name");
            return View(product);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id,[Bind("Price, Name, Description, IdBrand, IdSize, IdMaterial")] Product product)
        {
            if(id != product.Id)
            {
                return NotFound();
            }
            if(product.Id != null)
            {
                try
                {
                    await _productServer.Update(product);
                }
                catch (Exception ex)
                {
                    return Content(ex.Message);
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdSize"] = new SelectList(_sizeServices.GetAllSizes().Result, "Id", "Weight", "Height", "Width");
            ViewData["IdBrand"] = new SelectList(_brandServices.GetAllBrands().Result, "Id", "Name");
            ViewData["IdMaterial"] = new SelectList(_materialServices.GetAllMaterials().Result, "Id", "Name");
            return View(product);
        }
        //
        public async Task<IActionResult> Delete(Guid id)
        {
            if (_productServer.GetAllProduct() == null)
            {
                return NotFound();
            }

            var product = await _productServer.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCom(Guid id)
        {
            if(_productServer.GetAllProduct() == null)
            {
                return Problem("Entity set 'ProductDetail'  is null.");
            }
            await _productServer.Delete(id);
            return RedirectToAction(nameof(Delete));
        }
    }
}
