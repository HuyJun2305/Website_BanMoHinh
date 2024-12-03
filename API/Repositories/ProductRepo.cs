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
            //if (await GetProductById(product.Id) != null) throw new DuplicateWaitObjectException($"Product : {product.Id} is existed!");
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
            var result = await _context.Products.Include(p => p.Brand).Include(p => p.Material).Include(p => p.Size).Include(p => p.Category).Include(p => p.Images).ToListAsync();
            return result;
        }
        public async Task<Product> GetProductById(Guid id)
        {
            return await _context.Products.Include(p => p.Brand).Include(p => p.Material).Include(p => p.Size).Include(p => p.Category).FirstOrDefaultAsync();
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }

        public async Task Update(Product product)
        {
			var existingProduct = await _context.Products
				.FirstOrDefaultAsync(p => p.Id == product.Id);

			if (existingProduct == null)
			{
				throw new KeyNotFoundException("Không tìm thấy sản phẩm với Id này!");
			}

			existingProduct.Name = product.Name;
			existingProduct.Description = product.Description;
			existingProduct.Price = product.Price;
			existingProduct.Stock = product.Stock;
			existingProduct.CategoryId = product.CategoryId;

			_context.Products.Update(existingProduct);

			await _context.SaveChangesAsync();
		}
        public async Task<List<Product>> GetFilteredProduct(string? searchQuery = null, Guid? sizeId = null, Guid? brandId = null, Guid? materialId = null, Guid? categoryId = null)
        {
            var query = _context.Products
                .Include(p => p.Size)
                .Include(p => p.Brand)
                .Include(p => p.Material).Include(p => p.Category)
                .AsQueryable();
            //lọc theo Size
            if (sizeId.HasValue)
            {
                query = query.Where(p=> p.Size.Id == sizeId.Value);
            }
            //Lọc thương hiệu 
            if (brandId.HasValue)
            {
                query = query.Where(p=>p.Brand.Id == brandId.Value);
            }
            //
            if (materialId.HasValue)
            {
                query = query.Where(p=>p.Material.Id == materialId.Value);
            }
            if (categoryId.HasValue)
            {
                query = query.Where(p=>p.Category.Id == categoryId.Value);
            }
            //tìm kiếm theo tên sản phẩm 
            if(!string.IsNullOrWhiteSpace(searchQuery))
            {
                query = query.Where(p=>p.Name.Contains(searchQuery));
            }
            return await query.ToListAsync();
        }
    }
}
