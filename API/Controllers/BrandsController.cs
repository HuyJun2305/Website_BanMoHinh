using API.IRepositories;
using API.Repositories;
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
        //List danh sách Brand
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
        //Tìm Brand theo id
        [HttpGet("{id}")]
        public async Task<ActionResult<Brand>> GetBrand(Guid id)
        {
            try
            {
                return await _brandRepo.GetBrandById(id);

            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }

        }
        //Sửa Brand
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBrand(Brand Brand)
        {
            try
            {
                await _brandRepo.Update(Brand);
                await _brandRepo.SaveChanges();
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }

            return Content("Success!");
        }
        //Thêm Brand
        [HttpPost]
        public async Task<ActionResult<Brand>> PostBrand(Brand Brand)
        {
            try
            {
                await _brandRepo.Create(Brand);
                await _brandRepo.SaveChanges();

            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }

            return Content("Success!");
        }
        //Xóa Brand
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBrand(Guid id)
        {
            try
            {
                await _brandRepo.Delete(id);
                await _brandRepo.SaveChanges();

            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }

            return Content("Success!");
        }
    }
}
