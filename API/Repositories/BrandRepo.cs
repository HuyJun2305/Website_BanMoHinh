using API.Data;
using API.IRepositories;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class BrandRepo : IBrandRepo
    {
        private readonly ApplicationDbContext _context;
        public BrandRepo(ApplicationDbContext applicationDbContext)
        {
            _context = applicationDbContext;
        }
        //
        public async Task Create(Brand data)
        {
            if (await GetBrandById(data.Id) != null) throw new DuplicateWaitObjectException($"Brand : {data.Id} is existed!");
            await _context.Brands.AddAsync(data);
        }
        //
        public async Task Delete(Guid id)
        {
            var data = await GetBrandById(id);
            if (data == null) throw new KeyNotFoundException("Not found this brand!");
            if (_context.Products.Where(p => p.BrandId == data.Id).Any()) throw new Exception("This brand has used for some product!");
            _context.Brands.Remove(data);
        }
        //
        public async Task<List<Brand>> GetAllBrands()
        {
            return await _context.Brands.ToListAsync();
        }
        //
        public async Task<Brand> GetBrandById(Guid id)
        {
            return await _context.Brands.FindAsync(id);
        }
        //
        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }
        //
        public async Task Update(Brand data)
        {
            if (await GetBrandById(data.Id) == null) throw new KeyNotFoundException("Not found this brand!");
            _context.Entry(data).State = EntityState.Modified;
        }
    }
}

