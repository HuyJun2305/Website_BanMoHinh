using Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using View.IServices;
using View.Services;
using View.ViewModels;

namespace View.Controllers
{
	public class CategoryController : Controller
	{
		private readonly ICategoryServices _categoryServices;

		public CategoryController(ICategoryServices category)
		{
			_categoryServices = category;
		}

		public async Task<IActionResult> Index(int currentPage = 1, int rowsPerPage = 10)
		{
			var materials = await _categoryServices.GetAllCategories();

			// Phân trang
			var totalMaterials = materials.Count();
			var totalPages = (int)Math.Ceiling((double)totalMaterials / rowsPerPage);
			var pagedMaterials = materials.Skip((currentPage - 1) * rowsPerPage).Take(rowsPerPage).ToList();

			var viewModel = new CategoryViewModel
			{
				Categories = pagedMaterials,
				NewCategories = new Category(),
			};

			ViewBag.CurrentPage = currentPage;
			ViewBag.RowsPerPage = rowsPerPage;
			ViewBag.TotalPages = totalPages;

			return View(viewModel);
		}

		// GET: Brands/Details/5
		public async Task<IActionResult> Details(Guid id)
		{
			if (_categoryServices.GetAllCategories() == null)
			{
				return NotFound();
			}

			var brand = await _categoryServices.GetCategoryById(id);
			if (brand == null)
			{
				return NotFound();
			}

			return View(brand);
		}

		// GET: Brands/Create
		public IActionResult Create()
		{
			return View();
		}

		// POST: Brands/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(CategoryViewModel categoryViewModel)
		{
			if (ModelState.IsValid)
			{
				categoryViewModel.NewCategories.Id = Guid.NewGuid();
				await _categoryServices.Create(categoryViewModel.NewCategories);
				return RedirectToAction(nameof(Index));
			}
			return RedirectToAction(nameof(Index));
		}

		// GET: Brands/Edit/5
		public async Task<IActionResult> Edit(Guid id)
		{
			if (_categoryServices.GetAllCategories() == null)
			{
				return NotFound();
			}

			var brand = await _categoryServices.GetCategoryById(id);
			if (brand == null)
			{
				return NotFound();
			}
			return View(brand);
		}

		// POST: Brands/Edit/5
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(Guid id, CategoryViewModel categoryViewModel)
		{
			if (ModelState.IsValid)
			{
				try
				{
					await _categoryServices.Update(categoryViewModel.NewCategories);
				}
				catch (DbUpdateConcurrencyException)
				{
					var existingMaterial = await _categoryServices.GetCategoryById(categoryViewModel.NewCategories.Id);
					if (existingMaterial == null)
					{
						return NotFound();
					}
					else
					{
						throw;
					}
				}
				categoryViewModel.Categories = await _categoryServices.GetAllCategories();
				return RedirectToAction(nameof(Index));
			}
			return BadRequest("Lỗi không sửa được");
		}

		// GET: Brands/Delete/5
		public async Task<IActionResult> Delete(Guid id)
		{
			if (_categoryServices.GetAllCategories() == null)
			{
				return NotFound();
			}

			var brand = await _categoryServices.GetCategoryById(id);
			if (brand == null)
			{
				return NotFound();
			}

			return View(brand);
		}

		// POST: Brands/Delete/5
		[HttpPost]
		public async Task<IActionResult> DeleteConfirmed(Guid id)
		{
			if (_categoryServices.GetAllCategories() == null)
			{
				return Problem("Entity set 'Brand'  is null.");
			}

			await _categoryServices.Delete(id);

			return RedirectToAction(nameof(Index));
		}
		[HttpPost]
		public async Task<IActionResult> ToggleStatus(Guid id)
		{
			var category = await _categoryServices.GetCategoryById(id);
			if (category == null)
			{
				return NotFound();
			}

            category.Status = !category.Status;
			await _categoryServices.Update(category);

			return RedirectToAction("Index");
		}
	}
}
