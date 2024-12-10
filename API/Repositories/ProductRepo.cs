using API.Data;
using API.IRepositories;
using Data.DTO;
using Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;

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

        public async Task Delete(Guid productId, Guid? sizeId)
        {
            // Lấy sản phẩm theo productId và bao gồm cả ProductSizes
            var product = await _context.Products
                .Include(p => p.ProductSizes)
                .ThenInclude(ps => ps.Size)
                .FirstOrDefaultAsync(p => p.Id == productId);

            if (product == null)
            {
                throw new KeyNotFoundException("Sản phẩm không tồn tại.");
            }

            if (sizeId == null)
            {
                // Nếu không có sizeId, xóa sản phẩm bình thường
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
            else
            {
                // Nếu có sizeId, xóa bản ghi trong bảng ProductSize
                var productSize = await _context.ProductSizes
                    .FirstOrDefaultAsync(ps => ps.ProductId == productId && ps.SizeId == sizeId);

                if (productSize != null)
                {
                    _context.ProductSizes.Remove(productSize);
                    await _context.SaveChangesAsync();
                }

                // Kiểm tra xem sản phẩm có còn size này không và nếu không thì xóa size khỏi sản phẩm
                var size = await _context.Sizes.FirstOrDefaultAsync(s => s.Id == sizeId);
                if (size != null)
                {
                    // Kiểm tra số lượng sản phẩm còn lại với sizeId này
                    var productSizeCount = await _context.ProductSizes
                        .CountAsync(ps => ps.SizeId == sizeId);

                    // Nếu không còn sản phẩm nào sử dụng size này, xóa size
                    if (productSizeCount == 0)
                    {
                        _context.Sizes.Remove(size);
                        await _context.SaveChangesAsync();
                    }
                }
            }

            // Sau khi xóa ProductSize và Size (nếu có), xóa luôn sản phẩm
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }
        public async Task<List<Product>> GetAllProduct()
        {
            var result = await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Material)
                .Include(p => p.Category)
                .Include(p => p.Images)
                .Include(p => p.ProductSizes)
                 .ThenInclude(ps => ps.Size)
                .ToListAsync();
            return result;
        }

        public async Task<Product> GetProductById(Guid id)
        {
            return await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Material)
                .Include(p => p.Category)
                .Include(p => p.Images)
                 .Include(p => p.ProductSizes).ThenInclude(ps => ps.Size)
                .FirstOrDefaultAsync(p => p.Id == id);
        }
        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }

        public async Task Update(ProductDto productDto)
        {
            // Lấy thông tin sản phẩm từ ProductDto
            var product = productDto.Product;

            // Tìm sản phẩm trong cơ sở dữ liệu
            var existingProduct = await _context.Products
                .FirstOrDefaultAsync(p => p.Id == product.Id);

            if (existingProduct == null)
            {
                throw new Exception("Sản phẩm không tồn tại.");
            }

            // Cập nhật thông tin cơ bản của sản phẩm
            existingProduct.Name = product.Name;
            existingProduct.Price = product.Price;
            existingProduct.Stock = product.Stock;
            existingProduct.Description = product.Description;
            existingProduct.BrandId = product.BrandId;
            existingProduct.MaterialId = product.MaterialId;
            existingProduct.CategoryId = product.CategoryId;

            // Lấy danh sách SizeId hiện tại trong bảng ProductSizes
            var currentProductSizes = await _context.ProductSizes
                .Where(ps => ps.ProductId == product.Id)
                .ToListAsync();

            var currentSizeIds = currentProductSizes.Select(ps => ps.SizeId).ToList();

            // Xác định những SizeId cần thêm và cần xóa
            var sizesToRemove = currentSizeIds.Except(productDto.SizeIds).ToList();
            var sizesToAdd = productDto.SizeIds.Except(currentSizeIds).ToList();

            // Xóa các mối quan hệ cũ không còn tồn tại
            if (sizesToRemove.Any())
            {
                var productSizesToRemove = currentProductSizes
                    .Where(ps => sizesToRemove.Contains(ps.SizeId))
                    .ToList();

                _context.ProductSizes.RemoveRange(productSizesToRemove);
            }

            // Thêm các mối quan hệ mới vào bảng ProductSizes
            if (sizesToAdd.Any())
            {
                var productSizesToAdd = sizesToAdd
                    .Select(sizeId => new ProductSize
                    {
                        ProductId = product.Id,
                        SizeId = sizeId
                    })
                    .ToList();

                _context.ProductSizes.AddRange(productSizesToAdd);
            }

            // Lưu thay đổi vào cơ sở dữ liệu
            await _context.SaveChangesAsync();
        }





        public async Task<List<Product>> GetFilteredProduct(string? searchQuery = null, Guid? brandId = null, Guid? materialId = null, Guid? categoryId = null)
        {
            var query = _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Material).Include(p => p.Category)
                .AsQueryable();

            //Lọc thương hiệu 
            if (brandId.HasValue)
            {
                query = query.Where(p => p.Brand.Id == brandId.Value);
            }
            //
            if (materialId.HasValue)
            {
                query = query.Where(p => p.Material.Id == materialId.Value);
            }
            if (categoryId.HasValue)
            {
                query = query.Where(p => p.Category.Id == categoryId.Value);
            }
            //tìm kiếm theo tên sản phẩm 
            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                query = query.Where(p => p.Name.Contains(searchQuery));
            }
            return await query.ToListAsync();
        }

        public async Task AddSize(Guid productId, Guid sizeId)
        {
            // Kiểm tra sản phẩm có tồn tại không
            var productExists = await _context.Products.AnyAsync(p => p.Id == productId);
            if (!productExists)
            {
                throw new ArgumentException("Mã sản phẩm không hợp lệ.");
            }

            // Kiểm tra size có tồn tại không
            var sizeExists = await _context.Sizes.AnyAsync(s => s.Id == sizeId);
            if (!sizeExists)
            {
                throw new ArgumentException("Size không tồn tại.");
            }

            // Kiểm tra xem mối quan hệ giữa sản phẩm và size đã tồn tại chưa
            var productSizeExists = await _context.ProductSizes
                                                  .AnyAsync(ps => ps.ProductId == productId && ps.SizeId == sizeId);
            if (productSizeExists)
            {
                throw new ArgumentException("Size đã được liên kết với sản phẩm.");
            }

            // Tạo mới mối quan hệ giữa sản phẩm và size
            var productSize = new ProductSize
            {
                ProductId = productId,
                SizeId = sizeId
            };
            _context.ProductSizes.Add(productSize);

            // Lưu thay đổi vào cơ sở dữ liệu
            await _context.SaveChangesAsync();
        }




    }

}