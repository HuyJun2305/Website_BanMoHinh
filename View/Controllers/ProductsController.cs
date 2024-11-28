using Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using View.IServices;
using View.Services;
using View.ViewModels;

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
                    Images = selectedImage,
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
            ViewData["SizeId"] = new SelectList(_sizeServices.GetAllSizes().Result, "Id", "Value");
            ViewData["BrandId"] = new SelectList(_brandServices.GetAllBrands().Result, "Id", "Name");
            ViewData["MaterialId"] = new SelectList(_materialServices.GetAllMaterials().Result, "Id", "Name");
            var dataImage = new ProductViewModel()
            {
                Images = _imageServices.GetAllImages().Result
            };
            return View(dataImage);
        }
        [HttpPost]
        public async Task<IActionResult> Create([Bind("Price, Name, Description, BrandId, SizeId, MaterialId")] Product product)
        {
            if (ModelState.IsValid)
            {
                product.Id = Guid.NewGuid();
                await _productServer.Create(product);

                return Json(new { success = true, productId = product.Id });
            }

            return Json(new { success = false, message = "Dữ liệu không hợp lệ." });
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
            return View(product);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Price, Name, Description, BrandId, SizeId, MaterialId")] Product product)
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


        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile imageFile, Guid productId)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                var extension = Path.GetExtension(imageFile.FileName).ToLower();
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };

                if (!allowedExtensions.Contains(extension))
                {
                    return Json(new { success = false, message = "File không hợp lệ" });
                }
                var guidForImg = Guid.NewGuid().ToString();
                // Lưu ảnh vào thư mục hoặc xử lý ảnh
                string dataPath = Path.Combine("/images/", guidForImg + "-" + imageFile.FileName);
                string filePath = Path.Combine("wwwroot/images", guidForImg + "-" + imageFile.FileName);
                var product = _productServer.GetAllProduct().Result;

                if (product == null)
                {
                    return Json(new { success = false, message = "Màu không hợp lệ" });
                }

                Image img = new Image()
                {
                    Id = Guid.NewGuid(),
                    ProductId = productId,
                    URL = dataPath,
                };
                try
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await _imageServices.Create(img);
                        await imageFile.CopyToAsync(stream);
                    }
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = $"{ex.Message}" });
                }

                return Json(new { success = true });
            }

            return Json(new { success = false, message = "Không có ảnh được tải lên" });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteImage(string id)
        {
            if (id == null) return Json(new { success = false, message = "Không có ảnh được chọn" });
            var img = await _imageServices.GetImageById(Guid.Parse(id));
            var imageName = img.URL.Split('/')[2];

            string filePath = Path.Combine("wwwroot/images", imageName);
            if (System.IO.File.Exists(filePath))
            {
                try
                {
                    System.IO.File.Delete(filePath);
                    await _imageServices.Delete(Guid.Parse(id));
                    return Json(new { success = true });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = "Không thể xoá ảnh: " + ex.Message });
                }
            }
            else
            {
                return Json(new { success = false, message = "Không thể xoá ảnh: Không tìm thấy ảnh" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> GetImages(Guid productId)
        {
            var images = _imageServices.GetAllImages().Result.Where(i => i.ProductId == productId);

            return Json(new { success = true, images });
        }
    }
}
