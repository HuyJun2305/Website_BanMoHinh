using Data.Models;
using Newtonsoft.Json;
using View.IServices;

namespace View.Servicecs
{
    public class MaterialServices : IMaterialServices
    {
        private readonly HttpClient _httpClient;
        public MaterialServices(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task Create(Material Material)
        {
            await _httpClient.PostAsJsonAsync("https://localhost:7280/api/Materials", Material);
        }

        public async Task Delete(Guid id)
        {
            await _httpClient.DeleteAsync($"https://localhost:7280/api/Materials/{id}");
        }

        public async Task<IEnumerable<Material>> GetAllMaterials()
        {
            var response = await _httpClient.GetStringAsync("https://localhost:7280/api/Materials");
            IEnumerable<Material>? Materials = JsonConvert.DeserializeObject<IEnumerable<Material>>(response);
            return Materials;
        }

        public async Task<Material> GetMaterialById(Guid id)
        {
            var response = await _httpClient.GetStringAsync($"https://localhost:7280/api/Materials/{id}");
            Material? Material = JsonConvert.DeserializeObject<Material>(response);
            return Material;
        }

        public async Task Update(Material Material)
        {
            await _httpClient.PutAsJsonAsync($"https://localhost:7280/api/Materials/{Material.Id}", Material);
        }
    }
}
