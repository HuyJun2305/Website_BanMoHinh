using API.IRepositories;
using Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SizesController : ControllerBase
    {
        private readonly ISizeRepo _SizeRepo;
        public SizesController(ISizeRepo SizeRepo)
        {
            _SizeRepo = SizeRepo;
        }

        //GET
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Size>>> GetSizes()
        {
            return await _SizeRepo.GetAllSize();
        }
        [HttpGet("GetSizeByStatus")]
        public async Task<ActionResult<IEnumerable<Size>>> GetSizeByStatus()
        {
            return await _SizeRepo.GetSizeByStatus();
        }
        //GET id
        [HttpGet("{id}")]
        public async Task<ActionResult<Size>> GetSize(Guid id)
        {
            return await _SizeRepo.GetSizeById(id);
        }
        //Thêm
        [HttpPost]
        public async Task<ActionResult<Size>> PostSize(Size Size)
        {
            try
            {
                await _SizeRepo.Create(Size);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
            await _SizeRepo.SaveChanges();
            return CreatedAtAction("GetSize", new { id = Size.Id }, Size);
        }
        //Sửa
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSize(Size size)
        {
            try
            {
                await _SizeRepo.Update(size);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
            await _SizeRepo.SaveChanges();
            return Content("Success!");
        }
        //Xóa
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSize(Guid id)
        {
            try
            {
                await _SizeRepo.Delete(id);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
            await _SizeRepo.SaveChanges();
            return Content("Success!");
        }

        [HttpGet("{productId}/GetSize")]
        public async Task<IActionResult> GetSizeByProductId(Guid productId)
        {
            var sizes = await _SizeRepo.GetSizeByProductId(productId);

            if (sizes == null)
            {
                return NotFound(new { message = "Không tìm thấy kích thước cho sản phẩm này." });
            }

            return Ok(sizes);
        }
    }
}
