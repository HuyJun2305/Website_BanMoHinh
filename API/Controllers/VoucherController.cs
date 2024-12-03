using API.IRepositories;
using Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VoucherController : ControllerBase
    {
        private readonly IVoucherRepos _voucherRepos;

        public VoucherController(IVoucherRepos voucherRepos)
        {
            _voucherRepos = voucherRepos;
        }
        // GET: api/voucher
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Voucher>>> GetAllVouchers()
        {
            return await _voucherRepos.GetAll();
        }

        // GET: api/voucher/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Voucher>> GetVoucherById(Guid id)
        {
            var voucher = await _voucherRepos.GetById(id);
            if (voucher == null)
            {
                return NotFound(new { Message = "Voucher not found." });
            }
            return Ok(voucher);
        }

        // POST: api/voucher
        [HttpPost]
        public async Task<ActionResult<Voucher>> CreateVoucher([FromBody] Voucher voucher)
        {
            if (voucher == null)
                return BadRequest(new { Message = "Invalid voucher data." });

            if (voucher.PriceReduced != null && voucher.PercentReduced != null || voucher.PriceReduced == null && voucher.PercentReduced == null)
            {
                return BadRequest(new { message = "can't exist PriceReduced and PercentReduced at same time. One of them at least have value" });
            }

            await _voucherRepos.create(voucher);
            return CreatedAtAction(nameof(GetVoucherById), new { id = voucher.Id }, voucher);
        }

        // PUT: api/voucher/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateVoucher(Guid id, [FromBody] Voucher voucher)
        {
            if (voucher == null || voucher.Id != id)
                return BadRequest(new { Message = "Invalid voucher data." });

            var existingVoucher = await _voucherRepos.GetById(id);
            if (existingVoucher == null)
            {
                return NotFound(new { Message = "Voucher not found." });
            }

            if (voucher.PriceReduced != null && voucher.PercentReduced != null || voucher.PriceReduced == null && voucher.PercentReduced == null)
            {
                return BadRequest(new { message = "can't exist PriceReduced and PercentReduced at same time. One of them at least have value" });
            }

            await _voucherRepos.update(voucher);
            return NoContent();
        }

        // DELETE: api/voucher/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteVoucher(Guid id)
        {
            var voucher = await _voucherRepos.GetById(id);
            if (voucher == null)
            {
                return NotFound(new { Message = "Voucher not found." });
            }

            await _voucherRepos.delete(id);
            return NoContent();
        }
        //lấy ra voucher có thể áp dụng
        // GET: api/voucher/applicable
        [HttpGet("applicable")]
        public async Task<ActionResult<IEnumerable<Voucher>>> GetApplicableVouchers()
        {
            var vouchers = await _voucherRepos.GetAll();
            var applicableVouchers = vouchers.Where(v => v.Status && v.DayStart <= DateTime.Now && v.DayEnd >= DateTime.Now && v.Stock > 0)
                .OrderByDescending(v =>  v.PercentReduced)
                .ToList();
            return Ok(applicableVouchers);
        }
    }
}
