using API.Data;
using API.IRepositories;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class ImageRepo : IImageRepo
    {
        private readonly ApplicationDbContext _context;
        public ImageRepo(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task Create(Image image)
        {
            if (await GetImageById(image.Id) != null) throw new DuplicateWaitObjectException($"Image : {image.Id} is existed!");
            Image data = new Image()
            {
                Id = image.Id,
                URL = image.URL,
                ProductId = image.ProductId,
            };
        }

        public async Task Delete(Guid id)
        {
            var Image = await GetImageById(id);
            if (Image == null) throw new KeyNotFoundException("Not found this Image!");
            _context.Images.Remove(Image);
        }

        public async Task<List<Image>> GetAllImage()
        {
            return await _context.Images.Include(p => p.Product).ToListAsync();
        }

        public async Task<Image> GetImageById(Guid id)
        {
            return await _context.Images.Include(p => p.Product).Where(p => p.Id == id).FirstOrDefaultAsync();
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }

        public async Task Update(Image image)
        {
            if (await GetImageById(image.Id) == null) throw new KeyNotFoundException("Not found this Image!");
            Image data = new Image()
            {
                Id = image.Id,
                URL = image.URL,
                ProductId = image.ProductId,
            };
            _context.Entry(data).State = EntityState.Modified;
        }
    }
}
