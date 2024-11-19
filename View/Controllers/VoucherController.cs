using Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using View.Iservices;
using View.ViewModel;

namespace View.Controllers
{
    public class VoucherController : Controller
    {
        private readonly IVoucherService _voucherService;

        public VoucherController(IVoucherService voucherService)
        {
            _voucherService = voucherService;
        }

        // GET: Vouchers
        public async Task<IActionResult> Index(string searchQuery, DateTime? fromDate, DateTime? toDate, string status, int currentPage = 1, int rowsPerPage = 10)
        {
            var vouchers = await _voucherService.GetAllVouchers();

            if (!string.IsNullOrEmpty(searchQuery))
            {
                searchQuery = searchQuery.ToLower();
                vouchers = vouchers.Where(v => v.Name.ToLower().Contains(searchQuery)).ToList();
            }

            if (fromDate.HasValue)
            {
                vouchers = vouchers.Where(v => v.DayStart >= fromDate.Value).ToList();
            }

            if (toDate.HasValue)
            {
                vouchers = vouchers.Where(v => v.DayEnd <= toDate.Value).ToList();
            }

            if (!string.IsNullOrEmpty(status))
            {
                bool isActive = status == "true";
                vouchers = vouchers.Where(v => v.Status == isActive).ToList();
            }

            // Phân trang
            int totalRecords = vouchers.Count();
            int totalPages = (int)Math.Ceiling((double)totalRecords / rowsPerPage);
            var paginatedVouchers = vouchers.Skip((currentPage - 1) * rowsPerPage).Take(rowsPerPage).ToList();

            // Truyền thông tin phân trang vào ViewBag
            ViewBag.CurrentPage = currentPage;
            ViewBag.TotalPages = totalPages;
            ViewBag.RowsPerPage = rowsPerPage;

            ViewData["CurrentSearchQuery"] = searchQuery;
            ViewData["CurrentFromDate"] = fromDate?.ToString("yyyy-MM-dd");
            ViewData["CurrentToDate"] = toDate?.ToString("yyyy-MM-dd");
            ViewData["CurrentStatus"] = status;

            return View(paginatedVouchers);
        }

        // GET: Vouchers/Details/5
        public async Task<IActionResult> Details(Guid id)
        {
            var voucher = await _voucherService.GetVoucherById(id);
            if (voucher == null)
            {
                return NotFound();
            }

            return View(voucher);
        }

        // GET: Vouchers/Create
        public async Task<IActionResult> Create()
        {
            var model = new VoucherViewModel
            {
                Voucher = new Voucher(),
                CurrentPage = 1,
                TotalPages = 1
            };
            return View(model);
        }

        // POST: Vouchers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VoucherViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.Voucher.Id = Guid.NewGuid();
                try
                {
                    await _voucherService.Create(model.Voucher);
                    return RedirectToAction(nameof(Index));
                }
                catch (ArgumentException ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(model);
        }




        // GET: Vouchers/Edit/5
        public async Task<IActionResult> Edit(Guid id)
        {
            var voucher = await _voucherService.GetVoucherById(id);
            if (voucher == null)
            {
                return NotFound();
            }
            return View(voucher);
        }

        // POST: Vouchers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name,Percent,Stock,Condition,DayStart,DayEnd,Status,AccountId")] Voucher voucher)
        {
            if (id != voucher.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _voucherService.Update(voucher);
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (await _voucherService.GetVoucherById(id) == null)
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
            return View(voucher);
        }

        // GET: Vouchers/Delete/5
        public async Task<IActionResult> Delete(Guid id)
        {
            var voucher = await _voucherService.GetVoucherById(id);
            if (voucher == null)
            {
                return NotFound();
            }

            return View(voucher);
        }

        // POST: Vouchers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            try
            {
                await _voucherService.Delete(id);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(await _voucherService.GetVoucherById(id));
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
