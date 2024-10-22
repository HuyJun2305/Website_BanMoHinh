using API.IRepositories;
using Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandsController : ControllerBase
    {
        private readonly IBrandRepo _brandRepo;
        public BrandsController(IBrandRepo brandRepo)
        {
            _brandRepo = brandRepo;
        }
        //List danh sách
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Brand>>> GetBrands()
        {
            try
            {
                return await _brandRepo.GetAllBrands();

            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
        //GET Id
        [HttpGet("{id}")]
        public async Task<ActionResult<Brand>> GetBrand(Guid id)
        {
            return await _brandRepo.GetBrandById(id);

        }
        //Thêm Brand
        [HttpPost]
        public async Task<ActionResult<Brand>> PostBrand(Brand Brand)
        {
            try
            {
                await _brandRepo.Create(Brand);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
            await _brandRepo.SaveChanges();
            return CreatedAtAction("GetBrand", new { id = Brand.Id }, Brand);
        }
        //Sửa Brand
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBrand(Brand Brand)
        {
            try
            {
                await _brandRepo.Update(Brand);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
            await _brandRepo.SaveChanges();
            return Content("Success!");
        }
        //Xóa Brand
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBrand(Guid id)
        {
            try
            {
                await _brandRepo.Delete(id);


            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
            await _brandRepo.SaveChanges();
            return Content("Success!");
        }
    }
}
