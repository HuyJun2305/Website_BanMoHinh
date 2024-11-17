using Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using View.IServices;
using View.Servicecs;
using View.ViewModel;

namespace View.Controllers
{
    public class BrandsController : Controller
    {
        private readonly IBrandServices _brandService;
        public BrandsController(IBrandServices brandServices)
        {
            _brandService = brandServices;
        }
        //Danh sách (GET)
        public async Task<IActionResult> Index(int currentPage = 1, int rowsPerPage = 10)
        {
            var all = await _brandService.GetAllBrands();
            //
            var totalAll = all.Count();
            var totalPages = (int)Math.Ceiling((double)totalAll / rowsPerPage);
            var pagedBrand = all.Skip((currentPage - 1) * rowsPerPage).Take(rowsPerPage).ToList();
            var viewModel = new BrandViewModel
            {
                brands = pagedBrand,
                Brand = new Brand(),
            };
            ViewBag.CurrentPage = currentPage;
            ViewBag.RowsPerPage = rowsPerPage;
            ViewBag.TotalPages = totalPages;
            return View(viewModel);
        }
        //Chi tiết 
        public async Task<IActionResult> Details(Guid id)
        {
            if (_brandService.GetAllBrands() == null)
            {
                return NotFound();
            }

            var brand = await _brandService.GetBrandById(id);
            if (brand == null)
            {
                return NotFound();
            }

            return View(brand);
        }
        //Thêm
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Guid id, Brand brand)
        {
            if (brand.Id != null)
            {
                brand.Id = Guid.NewGuid();
                await _brandService.Create(brand);
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }
        //Sửa
        public async Task<IActionResult> Edit(Guid id)
        {
            if (_brandService.GetAllBrands() == null)
            {
                return NotFound();
            }

            var brand = await _brandService.GetBrandById(id);
            if (brand == null)
            {
                return NotFound();
            }
            return View(brand);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Brand brand)
        {
            if (id != brand.Id)
            {
                return NotFound();
            }
            if(brand.Id != null)
            { 
                try
                {
                    await _brandService.Update(brand);
                }
                catch (Exception ex)
                {
                    return Content(ex.Message);
                }
                return RedirectToAction(nameof(Index));
            }
            return BadRequest("Lỗi không sửa được");
        }
        //Xóa
        public async Task<IActionResult> Delete(Guid id)
        {
            if (_brandService.GetAllBrands() == null)
            {
                return NotFound();
            }

            var brand = await _brandService.GetBrandById(id);
            if (brand == null)
            {
                return NotFound();
            }

            return View(brand);
        }
        //
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_brandService.GetAllBrands() == null)
            {
                return Problem("Entity set 'Brand'  is null.");
            }

            await _brandService.Delete(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
