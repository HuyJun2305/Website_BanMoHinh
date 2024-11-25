using Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using View.IServices;
using View.ViewModels;

namespace View.Controllers
{
    public class PromotionController : Controller
    {
        private readonly IPromotionService _promotionService;

        public PromotionController(IPromotionService promotionService)
        {
            _promotionService = promotionService;
        }
        // GET: Promotions
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var viewContext = _promotionService.GetAllPromotion().Result;
            if (viewContext == null) return View("'Promotion is null!'");
            return View(viewContext.ToList());
        }

        //  GET: Promotions/Details/5

        public async Task<IActionResult> Details(Guid id)
        {
            var promotion = await _promotionService.GetPromotionById(id);
            if (promotion == null)
            {
                return NotFound();
            }
            return View(promotion);
        }

        // GET: Promotions/Create
        public async Task<IActionResult> Create()
        {
            var model = new PromotionViewModel
            {
                Promotion = new Promotion(),
                CurrentPage = 1,
                TotalPages = 1
            };
            return View(model);

        }


        // POST: Promotions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PromotionViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.Promotion.Id = Guid.NewGuid();
                try
                {
                    await _promotionService.Create(model.Promotion);
                    return RedirectToAction(nameof(Index));
                }
                catch (ArgumentException ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(model);
        }

        // GET: Promotions/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            var promotion = await _promotionService.GetPromotionById(id);
            if (promotion == null)
            {
                return NotFound();
            }
            return View(promotion);
        }

        // POST: Promotions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name,Description,DayStart,DayEnd,Status")] Promotion promotion)
        {
            if (id != promotion.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _promotionService.Update(promotion);
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (await _promotionService.GetPromotionById(id) == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (ArgumentException ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(promotion);
        }

        // GET: Promotions/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {


            var promotion = await _promotionService.GetPromotionById(id);
            if (promotion == null)
            {
                return NotFound();
            }


            return View(promotion);
        }

        // POST: Promotions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            try
            {
                await _promotionService.Delete(id);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(await _promotionService.GetPromotionById(id));
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
