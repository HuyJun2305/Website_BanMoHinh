using Data.DTO;
using Data.Models;

namespace API.IRepositories
{
    public interface IProductRepos
    {
        Task<List<Product>> GetAllProduct();
        Task<List<ProductSize>> GetAllProductSizes();
        Task<Product> GetProductById(Guid id);
        Task Create(Product product);
        Task Update(ProductDto productDto);
        Task Delete(Guid productId);
        Task SaveChanges();
        Task<List<Product>> GetSearch(string? searchQuery = null);
        Task<List<Product>> GetFilterProducts(List<string>? material,
	List<string>? size,
	List<string>? brands,
	string? priceRange);
        Task AddSize(Guid productId, Guid sizeId);
        Task DistributeStockToProductSizesAsync(Guid productId, int totalStock, Dictionary<Guid, int> productSizesStock);
    }
}

