using Data.DTO;
using Data.Models;

namespace API.IRepositories
{
    public interface IProductRepos
    {
        Task<List<Product>> GetAllProduct();
        Task<Product> GetProductById(Guid id);
        Task Create(Product product);
        Task Update(ProductDto productDto);
        Task Delete(Guid productId, Guid? sizeId);
        Task SaveChanges();
        Task<List<Product>> GetFilteredProduct(string? searchQuery = null,  Guid? brandId = null, Guid? materialId = null, Guid? categoryId = null);
        Task AddSize(Guid productId, Guid sizeId);
        Task DistributeStockToProductSizesAsync(Guid productId, int totalStock, Dictionary<Guid, int> productSizesStock);
    }
}

