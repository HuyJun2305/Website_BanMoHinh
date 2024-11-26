using Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using View.IServices;
using View.Servicecs;

namespace View.Controllers
{
    public class ImageController : Controller
    {
        private readonly IImageServices _imageServices;
        private readonly IProductServices _productServices;
        public ImageController(IImageServices imageServices, IProductServices productServices)
        {
            _imageServices = imageServices;
            _productServices = productServices;
        }
        public async Task<IActionResult> Index()
        {
            ViewData["ProductId"] = new SelectList(await _productServices.GetAllProduct(), "Id", "Name");
            var viewImage = await _imageServices.GetAllImages();
            if(viewImage == null || !viewImage.Any())
            {
                ViewBag.Message = "No product details found.";
                return View(new List<Image>());
            } 
            return View(viewImage);
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var image = _imageServices.GetImageById(id).Result;
            return View(image);
        }
        public IActionResult Create()
        {
            ViewData["ProductId"] = new SelectList(_productServices.GetAllProduct().Result, "Id", "Name");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("URL ,ProductId")] Image image)
        {
            if(image.Id != null)
            {
                image.Id = Guid.NewGuid();
                await _imageServices.Create(image);
                return RedirectToAction(nameof(Index));
            }
            return View(image);
        }
        public async Task<IActionResult> Edit(Guid id)
        {
            if(_imageServices.GetAllImages()== null)
            {
                return NotFound();
            }
            var img = await _imageServices.GetImageById(id);
            if(img == null)
            {
                return NotFound();
            }
            ViewData["ProductId"] = new SelectList(_productServices.GetAllProduct().Result, "Id", "Name");
            return View(img);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("URL ,ProductId")] Image image)
        {
            if(id != image.Id)
            {
                return NotFound();
            }
            if(image.Id != null)
            {
                try
                {
                    await _imageServices.Update(image);
                }
                catch (Exception ex)
                {
                    return Content(ex.Message);
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductId"] = new SelectList(_productServices.GetAllProduct().Result, "Id", "Name");
            return View(image);
        }
        public async Task<IActionResult> Delete(Guid id)
        {
            if(_imageServices.GetAllImages()== null)
            {
                return NotFound();
            }
            var img = await _imageServices.GetImageById(id);
            if(img == null) 
            {
                return NotFound();
            }
            return View(img);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_imageServices.GetAllImages() == null)
            {
                return Problem("Entity set 'Image'  is null.");
            }
            await _imageServices.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
