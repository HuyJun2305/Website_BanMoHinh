using Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using View.IServices;
using View.ViewModel;

namespace View.Controllers
{
    public class MaterialsController : Controller
    {
        private readonly IMaterialServices _materialServices;
        public MaterialsController(IMaterialServices materialServices)
        {
            _materialServices = materialServices;
        }
        // list 
        public async Task<IActionResult> Index(int currentPage = 1, int rowsPerPage = 10)
        {
            var materials = await _materialServices.GetAllMaterials();

            // Phân trang
            var totalMaterials = materials.Count();
            var totalPages = (int)Math.Ceiling((double)totalMaterials / rowsPerPage);
            var pagedMaterials = materials.Skip((currentPage - 1) * rowsPerPage).Take(rowsPerPage).ToList();

            var viewModel = new MaterialViewModel
            {
                Materials = pagedMaterials,
                NewMaterial = new Material(),
            };

            ViewBag.CurrentPage = currentPage;
            ViewBag.RowsPerPage = rowsPerPage;
            ViewBag.TotalPages = totalPages;

            return View(viewModel);
        }
        //chi tiet
        public async Task<IActionResult> Details(Guid id)
        {
            if (_materialServices.GetAllMaterials() == null)
            {
                return NotFound();
            }

            var material = await _materialServices.GetMaterialById(id);
            if (material == null)
            {
                return NotFound();
            }

            return View(material);
        }
        //Them
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Guid id, Material material)
        {
            if (material.Id != null)
            {
                material.Id = Guid.NewGuid();
                await _materialServices.Create(material);
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }
        //Sua
        public async Task<IActionResult> Edit(Guid id)
        {
            if (_materialServices.GetAllMaterials() == null)
            {
                return NotFound();
            }

            var material = await _materialServices.GetMaterialById(id);
            if (material == null)
            {
                return NotFound();
            }
            return View(material);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Material material)
        {
            if (id != material.Id)
            {
            }
            if(material.Id != null)
            { 
                try
                {
                    await _materialServices.Update(material);
                }
                catch (Exception ex)
                {
                    return Content(ex.Message);
                }
                return RedirectToAction(nameof(Index));
            }
            return BadRequest("Lỗi không sửa được");
        }
        //xoa
        public async Task<IActionResult> Delete(Guid id)
        {
            if (_materialServices.GetAllMaterials() == null)
            {
                return NotFound();
            }

            var material = await _materialServices.GetMaterialById(id);
            if (material == null)
            {
                return NotFound();
            }

            return View(material);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_materialServices.GetAllMaterials() == null)
            {
                return Problem("Entity set 'Material'  is null.");
            }

            await _materialServices.Delete(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
