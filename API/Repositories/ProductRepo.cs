using API.Data;
using API.IRepositories;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class ProductRepos : IProductRepos
    {
        private readonly ApplicationDbContext _context;
        public ProductRepos(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task Create(Product product)
        {
            if (await GetProductById(product.Id) != null) throw new DuplicateWaitObjectException($"Product : {product.Id} is existed!");
            await _context.Products.AddAsync(product);
        }

        public async Task Delete(Guid id)
        {
            var product = await GetProductById(id);
            if (product == null) throw new KeyNotFoundException("Not found this Id!");
            _context.Products.Remove(product);
        }

        public async Task<List<Product>> GetAllProduct()
        {
            return await _context.Products.Include(p => p.Brand).Include(p => p.Material).Include(p => p.Size).ToListAsync();
        }

        public async Task<Product> GetProductById(Guid id)
        {
            return await _context.Products.Include(p => p.Brand).Include(p => p.Material).Include(p => p.Size).FirstOrDefaultAsync();
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }

        public async Task Update(Product product)
        {
            if (await GetProductById(product.Id) == null) throw new KeyNotFoundException("Not found this Id!");
            _context.Entry(product).State = EntityState.Modified;
        }
    }
}
