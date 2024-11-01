using API.IRepositories;
using Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImageRepo _imageRepo;
        public ImagesController(IImageRepo imageRepo)
        {
            _imageRepo = imageRepo;
        }
        //
        [HttpGet]
        public async Task<ActionResult<List<Image>>> GetAllImage()
        {
            return await _imageRepo.GetAllImage();
        }
        //
        [HttpGet("{id}")]
        public async Task<ActionResult<Image>> GetImageById(Guid id)
        {
            return await _imageRepo.GetImageById(id);
        }
        //
        [HttpPost]
        public async Task<IActionResult> PostImage(Image image)
        {
            try
            {
                Image data = new Image()
                {
                    Id = image.Id,
                    URL = image.URL,
                    ProductId = image.ProductId,
                };
                await _imageRepo.Create(data);
                await _imageRepo.SaveChanges();

            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
            return Content("Success!");
        }
        //
        [HttpPut("{id}")]
        public async Task<IActionResult> PutImage(Image image)
        {
            try
            {
                Image data = new Image()
                {
                    Id = image.Id,
                    URL = image.URL,
                    ProductId = image.ProductId,
                };
                await _imageRepo.Update(data);
                await _imageRepo.SaveChanges();
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
            return Content("Success!");
        }
        //
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            try
            {
                await _imageRepo.Delete(id);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
            _imageRepo.SaveChanges();
            return Content("Success!");
        }
    }
}
