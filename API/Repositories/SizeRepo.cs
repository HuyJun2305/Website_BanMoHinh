using API.Data;
using API.IRepositories;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class SizeRepo : ISizeRepo
    {
        private readonly ApplicationDbContext _context;
        public SizeRepo(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task Create(Size size)
        {
            if (await GetSizeById(size.Id) != null) throw new DuplicateWaitObjectException($"Size : {size.Id} is existed");
            await _context.Sizes.AddAsync(size);
        }

        public async Task Delete(Guid id)
        {
            var size = await GetSizeById(id);
            _context.Sizes.Remove(size);
        }

        public async Task<List<Size>> GetAllSize()
        {
            return await _context.Sizes.ToListAsync();
        }

        public async Task<Size> GetSizeById(Guid id)
        {
            return await _context.Sizes.FindAsync(id);
        }

        public async Task<List<Size>> GetSizeByProductId(Guid productId)
        {
            var product = await _context.Products
                                         .Where(p => p.Id == productId)
                                         .Include(p => p.Sizes) 
                                         .FirstOrDefaultAsync();

            // Kiểm tra nếu sản phẩm không tồn tại hoặc không có sizes
            if (product == null || product.Sizes == null)
            {
                return new List<Size>(); // Trả về danh sách rỗng nếu không có sản phẩm hoặc sizes
            }

            return product.Sizes.Where(s => s.Status).ToList();
        }

        public async Task<List<Size>> GetSizeByStatus()
		{
			return await _context.Sizes.Where(s => s.Status == true).ToListAsync();
		}

		public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }

        public async Task Update(Size size)
        {
            if (await GetSizeById(size.Id) == null) throw new KeyNotFoundException("Not found this size!");
            _context.Entry(size).State = EntityState.Modified;
        }
    }
}

