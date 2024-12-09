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

            var sp = new Product()
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Stock = product.Stock,
                Description = product.Description,
                BrandId = product.BrandId,
                MaterialId = product.MaterialId,
                CategoryId = product.CategoryId,
                Promotions = product.Promotions,

            };
            await _context.Products.AddAsync(sp);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(Guid id)
        {
            var product = _context.Products.Find(id);
            if (product == null) throw new KeyNotFoundException("Not found this Id!");
            _context.Products.Remove(product);
            _context.SaveChanges();
        }

        public async Task<List<Product>> GetAllProduct()
        {
            var result = await _context.Products.Include(p => p.Brand)
                .Include(p => p.Material)
                .Include(p => p.Sizes)
                .Include(p => p.Category)
                .Include(p => p.Images)
                .ToListAsync();
            return result;
        }
        public async Task<Product> GetProductById(Guid id)
        {
            return await _context.Products
                .Include(p => p.Brand)    
                .Include(p => p.Material)
                .Include(p => p.Sizes)
                .Include(p => p.Category)
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.Id == id); 
        }


        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }

        public async Task Update(Product product)
        {
            var existingProduct = await _context.Products.FindAsync(product.Id);
            if (existingProduct == null)
            {
                throw new Exception("Sản phẩm không tồn tại.");
            }

            // Cập nhật các trường cần thiết
            existingProduct.Name = product.Name;
            existingProduct.Price = product.Price;
            existingProduct.Stock = product.Stock;
            existingProduct.Description = product.Description;
            existingProduct.BrandId = product.BrandId;
            existingProduct.MaterialId = product.MaterialId;
            existingProduct.CategoryId = product.CategoryId;
            existingProduct.Promotions = product.Promotions;
            existingProduct.Images = product.Images;
            existingProduct.Sizes = product.Sizes;

            _context.Products.Update(existingProduct);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Product>> GetFilteredProduct(string? searchQuery = null,  Guid? brandId = null, Guid? materialId = null, Guid? categoryId = null)
        {
            var query = _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Material).Include(p => p.Category)
                .AsQueryable();

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
