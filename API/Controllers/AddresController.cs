using API.IRepositories;
using Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddresController : ControllerBase
    {
        private readonly IAddresRepo _addresRepo;

        public AddresController(IAddresRepo addresRepo)
        {
            _addresRepo = addresRepo;
        }
        //List danh sách Brand
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Address>>> GetAddress()
        {
            try
            {
                return await _addresRepo.GetAllAddres();

            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
        //Tìm Brand theo id
        [HttpGet("{id}")]
        public async Task<ActionResult<Address>> GetAddres(Guid id)
        {
            try
            {
                return await _addresRepo.GetAddresById(id);

            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }

        }
        //Sửa Brand
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAddres(Address address)
        {
            try
            {
                await _addresRepo.Update(address);
                await _addresRepo.SaveChanges();
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }

            return Content("Success!");
        }
        //Thêm Brand
        [HttpPost]
        public async Task<ActionResult<Brand>> PostAddres(Address address)
        {
            try
            {
                await _addresRepo.Create(address);
                await _addresRepo.SaveChanges();

            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }

            return Content("Success!");
        }
        //Xóa Brand
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAddres(Guid id)
        {
            try
            {
                await _addresRepo.Delete(id);
                await _addresRepo.SaveChanges();

            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }

            return Content("Success!");
        }
    }
}
