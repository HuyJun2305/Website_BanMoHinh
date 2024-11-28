using Data.Models;
using Newtonsoft.Json;
using View.IServices;

namespace View.Services
{
    public class ProductServices : IProductServices
    {
        private readonly HttpClient _httpClient;
        public ProductServices(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task Create(Product product)
        {
            await _httpClient.PostAsJsonAsync("https://localhost:7280/api/Products", product);
        }

        public async Task Delete(Guid id)
        {
            await _httpClient.DeleteAsync($"https://localhost:7280/api/Products/{id}");
        }

        public async Task<IEnumerable<Product>?> GetAllProduct()
        {
            var response = await _httpClient.GetStringAsync("https://localhost:7280/api/Products");
            var products = JsonConvert.DeserializeObject<IEnumerable<Product>>(response);
            return products;
        }

        public Task<List<Product>> GetFilteredProduct(Guid? searchQuery = null, Guid? sizeId = null, Guid? imageId = null, Guid? brandId = null, Guid? materialId = null)
        {
            throw new NotImplementedException();
        }

        public async Task<Product?> GetProductById(Guid id)
        {
            var response = await _httpClient.GetStringAsync($"https://localhost:7280/api/Products/{id}");
            var product = JsonConvert.DeserializeObject<Product>(response);
            return product;
        }

        public async Task Update(Product product)
        {
            await _httpClient.PutAsJsonAsync($"https://localhost:7280/api/Products/{product.Id}", product);
        }
    }
}
