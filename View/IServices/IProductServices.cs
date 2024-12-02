using Data.Models;

namespace View.IServices
{
    public interface IProductServices
    {
        Task<IEnumerable<Product>> GetAllProduct();
        Task<Product> GetProductById(Guid id);
        Task Create(Product product);
        Task Update(Product product);
        Task Delete(Guid id);
        Task<List<Product>> GetFilteredProduct(string? searchQuery = null,
            Guid? sizeId = null,
               Guid? imageId = null,
               Guid? brandId = null,Guid? categoryId = null,
               Guid? materialId = null);
    }
}
