using API.Data;
using API.IRepositories;
using Data.Models;
using Microsoft.AspNetCore.Mvc;
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
                .Include(p => p.Sizes) 
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
            // Tìm sản phẩm trong bảng Products và bao gồm Sizes
            var existingProduct = await _context.Products
                .Include(p => p.Sizes) // Đảm bảo lấy thông tin Sizes
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
            existingProduct.Promotions = product.Promotions;
            existingProduct.Images = product.Images;

            var currentSizeIds = existingProduct.Sizes.Select(s => s.Id).ToList();
            var newSizeIds = product.Sizes.Select(s => s.Id).ToList();

            var sizesToRemove = currentSizeIds.Except(newSizeIds).ToList(); // Những size đã bị xóa
            var sizesToAdd = newSizeIds.Except(currentSizeIds).ToList(); // Những size mới cần thêm

            if (!sizesToRemove.Any() && !sizesToAdd.Any())
            {
                throw new Exception("Không có thay đổi nào về size.");
            }

            // Bước 1: Xóa những Size không còn trong danh sách cập nhật
            if (sizesToRemove.Any())
            {
                foreach (var sizeId in sizesToRemove)
                {
                    var productSizeToRemove = await _context.ProductSizes
                        .FirstOrDefaultAsync(ps => ps.ProductId == product.Id && ps.SizeId == sizeId);

                    if (productSizeToRemove != null)
                    {
                        _context.ProductSizes.Remove(productSizeToRemove); // Xóa mối quan hệ size đã bị xóa
                    }
                    else
                    {
                        // Nếu không tìm thấy mối quan hệ size để xóa, ghi log cảnh báo
                        Console.WriteLine($"Cảnh báo: Không tìm thấy mối quan hệ size với SizeId {sizeId} để xóa.");
                    }
                }

                // Lưu thay đổi của bảng ProductSizes ngay sau khi xóa
                await _context.SaveChangesAsync();
            }

            if (sizesToAdd.Any())
            {
                foreach (var sizeId in sizesToAdd)
                {
                    // Thêm mối quan hệ mới vào bảng ProductSizes nếu chưa có
                    var existingProductSize = await _context.ProductSizes
                        .FirstOrDefaultAsync(ps => ps.ProductId == product.Id && ps.SizeId == sizeId);

                    if (existingProductSize == null)
                    {
                        // Nếu chưa có mối quan hệ, thêm mới
                        _context.ProductSizes.Add(new ProductSize { ProductId = product.Id, SizeId = sizeId });
                    }
                    else
                    {
                        // Nếu đã có, bỏ qua
                        Console.WriteLine($"Cảnh báo: Mối quan hệ size với SizeId {sizeId} đã tồn tại.");
                    }
                }
            }

            if (sizesToRemove.Any())
            {
                foreach (var sizeId in sizesToRemove)
                {
                    // Kiểm tra nếu size còn sản phẩm nào khác liên kết không
                    var isSizeLinkedToOtherProducts = await _context.ProductSizes
                        .AnyAsync(ps => ps.SizeId == sizeId);

                    if (!isSizeLinkedToOtherProducts)
                    {
                        var sizeToUpdate = await _context.Sizes
                            .FirstOrDefaultAsync(s => s.Id == sizeId);

                        if (sizeToUpdate != null)
                        {
                            sizeToUpdate.ProductId = null; // Xóa ProductId
                            _context.Sizes.Update(sizeToUpdate);
                        }
                    }
                }

                // Lưu thay đổi vào bảng Sizes
                await _context.SaveChangesAsync();
            }

            // Cập nhật lại danh sách Sizes của sản phẩm nếu không còn size nào
            if (!existingProduct.Sizes.Any()) 
            {
                existingProduct.Sizes = new List<Size>(); 
            }

            // Lưu tất cả thay đổi vào cơ sở dữ liệu
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
            var product = await _context.Products
                                        .Include(p => p.Sizes) // Bao gồm danh sách size liên kết
                                        .FirstOrDefaultAsync(p => p.Id == productId);
            if (product == null)
            {
                throw new ArgumentException("Mã sản phẩm không hợp lệ.");
            }

            // Lấy size theo sizeId
            var existingSize = await _context.Sizes.FindAsync(sizeId);
            if (existingSize == null)
            {
                throw new ArgumentException("Size không tồn tại.");
            }

            if (product.Sizes.Any(s => s.Id == sizeId))
            {
                return;
            }

            var existingProductSize = await _context.ProductSizes
                                                     .FirstOrDefaultAsync(ps => ps.ProductId == productId && ps.SizeId == sizeId);
            if (existingProductSize != null)
            {
                throw new ArgumentException("Đã tồn tại sản phẩm thuộc size không tồn tại.");
            }
            var productSize = new ProductSize
            {
                ProductId = productId,
                SizeId = sizeId
            };
            _context.ProductSizes.Add(productSize);
            product.Sizes.Add(existingSize);
            _context.Products.Update(product); // Cập nhật sản phẩm với danh sách sizes mới
            await _context.SaveChangesAsync();
        }




    }

}