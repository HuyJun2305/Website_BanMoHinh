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

        public Task<List<Size>> GetSizeByProductId(Guid productId)
        {
            throw new NotImplementedException();
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

