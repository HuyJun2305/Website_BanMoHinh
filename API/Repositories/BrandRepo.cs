using API.Data;
using API.IRepositories;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class BrandRepo : IBrandRepo
    {
        private readonly ApplicationDbContext _context;
        public BrandRepo(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task Create(Brand brand)
        {
            if (await GetBrandById(brand.Id) != null) throw new DuplicateWaitObjectException($"Brand : {brand.Id}is existed!");
            await _context.Brands.AddAsync(brand);
        }

        public async Task Delete(Guid id)
        {
            var brand = await GetBrandById(id);
            if (brand == null) throw new KeyNotFoundException("Not found this brand!");
            if (_context.Products.Where(p => p.IdBrand == brand.Id).Any()) throw new Exception("This brand has used for some product!");
            _context.Brands.Remove(brand);
        }

        public async Task<List<Brand>> GetAllBrands()
        {
            return await _context.Brands.ToListAsync();
        }

        public async Task<Brand> GetBrandById(Guid id)
        {
            return await _context.Brands.FindAsync();
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }

        public async Task Update(Brand brand)
        {
            if (await GetBrandById(brand.Id) == null) throw new KeyNotFoundException("Not found this brand!");
            _context.Entry(brand).State = EntityState.Modified;
        }
    }
}
