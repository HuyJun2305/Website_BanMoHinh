using Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using View.IServices;
using View.ViewModel;

namespace View.Controllers
{
    public class SizesController : Controller
    {
        private readonly ISizeServices _sizeServices;

        public SizesController(ISizeServices sizeServices)
        {
            _sizeServices = sizeServices;
        }
        // GET: Sizes
        public async Task<IActionResult> Index(int currentPage = 1, int rowsPerPage = 10)
        {
            var sizes = await _sizeServices.GetAllSizes();

            // Phân trang
            var totalSizes = sizes.Count();
            var totalPages = (int)Math.Ceiling((double)totalSizes / rowsPerPage);
            var pagedSizes = sizes.Skip((currentPage - 1) * rowsPerPage).Take(rowsPerPage).ToList();

            var viewModel = new SizeViewModel
            {
                Sizes = pagedSizes,
                NewSize = new Size(),
            };

            ViewBag.CurrentPage = currentPage;
            ViewBag.RowsPerPage = rowsPerPage;
            ViewBag.TotalPages = totalPages;

            return View(viewModel);
        }
        // GET: Sizes/Details/5
        public async Task<IActionResult> Details(Guid id)
        {
            if (_sizeServices.GetAllSizes == null)
            {
                return NotFound();
            }

            var size = await _sizeServices.GetSizeById(id);
            if (size == null)
            {
                return NotFound();
            }

            return View(size);
        }

        // GET: Sizes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Sizes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Guid id , Size size)
        {
            if (size.Id != null)
            {
                size.Id = Guid.NewGuid();
                await _sizeServices.Create(size);
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Sizes/Edit/5
        public async Task<IActionResult> Edit(Guid id)
        {
            if (_sizeServices.GetAllSizes() == null)
            {
                return NotFound();
            }

            var size = await _sizeServices.GetSizeById(id);
            if (size == null)
            {
                return NotFound();
            }
            return View(size);
        }

        // POST: Sizes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Size size)
        {
            if (id != size.Id)
            {
                return NotFound();
            }
            if (size.Id != null) 
            { 
                try
                {
                    await _sizeServices.Update(size);
                }
                catch (Exception ex)
                {
                    return Content(ex.Message);
                }
                return RedirectToAction(nameof(Index));
            }
            return BadRequest("Lỗi không sửa được");
        }

        // GET: Sizes/Delete/5
        public async Task<IActionResult> Delete(Guid id)
        {
            if (_sizeServices.GetAllSizes() == null)
            {
                return NotFound();
            }

            var size = await _sizeServices.GetSizeById(id);
            if (size == null)
            {
                return NotFound();
            }

            return View(size);
        }

        // POST: Sizes/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_sizeServices.GetAllSizes() == null)
            {
                return Problem("Entity set 'Size'  is null.");
            }

            await _sizeServices.Delete(id);

            return RedirectToAction(nameof(Index));
        }

    }
}
