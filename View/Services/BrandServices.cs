using Data.Models;
using Newtonsoft.Json;
using View.IServices;


namespace View.Services
{
    public class BrandServices : IBrandServices
    {
        private readonly HttpClient _httpClient;
        public BrandServices(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task Create(Brand Brand)
        {
            await _httpClient.PostAsJsonAsync("https://localhost:7280/api/Brands", Brand);
        }

        public async Task Delete(Guid id)
        {
            await _httpClient.DeleteAsync($"https://localhost:7280/api/Brands/{id}");
        }

        public async Task<IEnumerable<Brand>> GetAllBrands()
        {
            var response = await _httpClient.GetStringAsync("https://localhost:7280/api/Brands");
            IEnumerable<Brand> Brands = JsonConvert.DeserializeObject<IEnumerable<Brand>>(response);
            return Brands;
        }

        public async Task<Brand> GetBrandById(Guid id)
        {
            var response = await _httpClient.GetStringAsync($"https://localhost:7280/api/Brands/{id}");
            Brand Brand = JsonConvert.DeserializeObject<Brand>(response);
            return Brand;
        }

        public async Task Update(Brand Brand)
        {
            await _httpClient.PutAsJsonAsync($"https://localhost:7280/api/Brands/{Brand.Id}", Brand);
        }
    }
}
