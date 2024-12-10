using Data.DTO;
using Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using View.IServices;
using View.ViewModels;

namespace View.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductServices _productServives;
        private readonly ISizeServices _sizeServices;
        private readonly IBrandServices _brandServices;
        private readonly IMaterialServices _materialServices;
        private readonly IImageServices _imageServices;
        private readonly ICategoryServices _categoryServices;
        public ProductsController(IProductServices productServices, ISizeServices sizeServices,
            IBrandServices brandServices, IMaterialServices materialServices,
            IImageServices imageServices, ICategoryServices categoryServices)
        {
            _productServives = productServices;
            _sizeServices = sizeServices;
            _brandServices = brandServices;
            _materialServices = materialServices;
            _imageServices = imageServices;
            _categoryServices = categoryServices;
        }
        //Tìm kiếm theo tên 
        public async Task<IActionResult> FilterProducts(string? searchQuery = null, Guid? sizeId = null, Guid? brandId = null, Guid? materialId = null)
        {
            var product = await _productServives.GetFilteredProduct(searchQuery, sizeId, brandId, materialId);
            return Json(product);
        }
        public async Task<IActionResult> Index()
        {
            // Lấy dữ liệu bất đồng bộ
            var brands = await _brandServices.GetAllBrands();
            var materials = await _materialServices.GetAllMaterials();
            var categories = await _categoryServices.GetAllCategories();
            var products = await _productServives.GetAllProduct();
            var selectedImage = await _imageServices.GetAllImages();
            var selectedSize = await _sizeServices.GetSizeByStatus();

            // Tạo ViewData cho các dropdown list
            ViewData["BrandId"] = new SelectList(brands, "Id", "Name");
            ViewData["MaterialId"] = new SelectList(materials, "Id", "Name");
            ViewData["CategoryId"] = new SelectList(categories, "Id", "Name");

            // Chuẩn bị dữ liệu cho View
            var productData = new ProductIndex()
            {
                Products = products,
                Images = selectedImage,
                Sizes = selectedSize,
                Brands = brands,
                Materials = materials,
                Categories = categories
            };

            // Trả về view với model dữ liệu
            return View(productData);
        }
        public async Task<IActionResult> GetProductById(Guid id)
        {
            var product = await _productServives.GetProductById(id);
            return Json(product);
        }
        public async Task<IActionResult> Details(Guid id)
        {
            var product = _productServives.GetProductById(id).Result;
            return View(product);


        }
        public async Task<IActionResult> Create()
        {
            // Lấy dữ liệu bất đồng bộ
            var brands = await _brandServices.GetAllBrands();
            var materials = await _materialServices.GetAllMaterials();
            var categories = await _categoryServices.GetAllCategories();
            var images = await _imageServices.GetAllImages();
            var sizes = await _sizeServices.GetSizeByStatus();

            // Tạo ViewData cho các dropdown list
            ViewData["BrandId"] = new SelectList(brands, "Id", "Name");
            ViewData["MaterialId"] = new SelectList(materials, "Id", "Name");
            ViewData["CategoryId"] = new SelectList(categories, "Id", "Name");
            ViewData["SizeId"] = new SelectList(sizes, "Id", "Value");

            // Tạo ViewModel và truyền vào view
            var dataImage = new ProductViewModel
            {
                Images = images,
                Sizes = sizes
            };

            return View(dataImage);
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("Price, Name, Description,Stock, BrandId, SizeId, MaterialId, CategoryId")] Product product)
        {
            if (ModelState.IsValid)
            {
                product.Id = Guid.NewGuid();
                await _productServives.Create(product);

                return Json(new { success = true, productId = product.Id });
            }

            return Json(new { success = false, message = "Dữ liệu không hợp lệ." });
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var product = await _productServives.GetProductById(id);

            if (product == null)
            {
                return NotFound("Không tìm thấy sản phẩm.");
            }
            return View(product);
        }


        [HttpPut]
        public async Task<IActionResult> Edit([FromBody] ProductDto productDto)
        {
            if (productDto == null)
            {
                return BadRequest("Invalid product data.");
            }

            // Cập nhật sản phẩm
            await _productServives.Update(productDto);
            return Json(new { success = true });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                // Lấy sản phẩm cần xóa
                var product = await _productServives.GetProductById(id);
                if (product == null)
                {
                    return Json(new { success = false, message = "Sản phẩm không tồn tại." });
                }

                // Xóa sản phẩm
                await _productServives.Delete(id);

                return Json(new { success = true, message = "Sản phẩm đã được xóa thành công." });
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu có
                return Json(new { success = false, message = $"Lỗi khi xóa sản phẩm: {ex.Message}" });
            }
        }


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

                var fileName = imageFile.FileName;
                var guidForImg = Guid.NewGuid().ToString();
                var filePath = Path.Combine("wwwroot/images", guidForImg + "-" + fileName);
                var dataPath = Path.Combine("/images/", guidForImg + "-" + fileName);

                // Kiểm tra xem ảnh đã tồn tại chưa
                if (System.IO.File.Exists(filePath))
                {
                    return Json(new { success = false, message = "Ảnh đã tồn tại trong hệ thống" });
                }

                var product = _productServives.GetAllProduct().Result;

                if (product == null)
                {
                    return Json(new { success = false, message = "Mã sản phẩm không hợp lệ" });
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





        [HttpGet("GetSizes")]
        public async Task<IActionResult> GetSizes()
        {
            try
            {
                var sizes = await _sizeServices.GetSizeByStatus();  // Gọi dịch vụ để lấy dữ liệu
                if (sizes == null || !sizes.Any())
                {
                    return NotFound("Không có kích thước nào được tìm thấy.");
                }

                return Json(sizes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi từ máy chủ: {ex.Message}");
            }
        }


        [HttpGet]
        public IActionResult CheckImageExists(string imageName)
        {
            var filePath = Path.Combine("wwwroot/images", imageName);

            if (System.IO.File.Exists(filePath))
            {
                return Json(new { exists = true });
            }

            return Json(new { exists = false });
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteImage(Guid id)
        {
            if (id == Guid.Empty)
                return Json(new { success = false, message = "ID không hợp lệ." });

            var img = await _imageServices.GetImageById(id);
            if (img == null)
                return Json(new { success = false, message = "Không tìm thấy ảnh." });

            try
            {
                await _imageServices.Delete(id);
                return Json(new { success = true, message = "Ảnh đã được xóa." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Lỗi khi xóa ảnh: " + ex.Message });
            }
        }


        [HttpGet]
        public async Task<IActionResult> GetImages(Guid productId)
        {
            var images = _imageServices.GetAllImages().Result.Where(i => i.ProductId == productId);

            return Json(new { success = true, images });
        }




    }
}
