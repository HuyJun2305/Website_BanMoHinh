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
            await _context.Images.AddAsync(image);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(Guid id)
        {
            var Image = await GetImageById(id);
            if (Image == null) throw new KeyNotFoundException("Not found this Image!");
            _context.Images.Remove(Image);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteImageInProduct(Guid productId, Guid imageId)
        {
            // Tìm ảnh thuộc sản phẩm với ID tương ứng
            var image = await _context.Images
                .FirstOrDefaultAsync(img => img.Id == imageId && img.ProductId == productId);

            if (image == null)
                throw new KeyNotFoundException("Không tìm thấy ảnh hoặc ảnh không thuộc sản phẩm này!");

            // Xóa ảnh và lưu thay đổi
            _context.Images.Remove(image);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Image>> GetAllImage()
        {
            return await _context.Images.Include(p => p.Product).ToListAsync();
        }

        public async Task<Image> GetImageById(Guid id)
        {
            return await _context.Images.Include(p => p.Product).Where(p => p.ProductId == id).FirstOrDefaultAsync();
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }

        public async Task Update(Image image)
        {
            if (await GetImageById(image.Id) == null) throw new KeyNotFoundException("Not found this Image!");
            _context.Entry(image).State = EntityState.Modified;

        }
    }
}
