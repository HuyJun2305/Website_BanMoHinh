using Data.Models;
using Newtonsoft.Json;
using View.IServices;

namespace View.Services
{
    public class PromotionService : IPromotionService
    {
        private readonly HttpClient _httpClient;

        public PromotionService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task Create(Promotion promotion)
        {
            await _httpClient.PostAsJsonAsync("https://localhost:7280/api/Promotion", promotion);
        }

        public async Task Delete(Guid id)
        {
            await _httpClient.DeleteAsync($"https://localhost:7280/api/Promotion/{id}");
        }

        public async Task<List<Promotion>> GetAllPromotion()
        {
            var response = await _httpClient.GetStringAsync("https://localhost:7280/api/Promotion");
            var listPromotion = JsonConvert.DeserializeObject<List<Promotion>>(response);
            return listPromotion;
        }

        public async Task<Promotion> GetPromotionById(Guid? id)
        {
            var response = await _httpClient.GetStringAsync($"https://localhost:7280/api/Promotion/{id}");
            var PromotionItem = JsonConvert.DeserializeObject<Promotion>(response);
            return PromotionItem;
        }

        public async Task Update(Promotion promotion)
        {
            await _httpClient.PutAsJsonAsync($"https://localhost:7280/api/Promotion/{promotion.Id}", promotion);
        }
    }
}
