using Data.Models;
using Newtonsoft.Json;
using View.Iservices;

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
            await _httpClient.PostAsJsonAsync("https://localhost:7170/api/Promotions", promotion);
        }

        public async Task Delete(Guid id)
        {
            await _httpClient.DeleteAsync($"https://localhost:7170/api/Promotions/{id}");
        }

        public async Task<List<Promotion>?> GetAllPromotion()
        {
            var response = await _httpClient.GetStringAsync("https://localhost:7170/api/Promotions");
            var listPromotion = JsonConvert.DeserializeObject<List<Promotion>>(response);
            return listPromotion;
        }

        public async Task<Promotion?> GetPromotionById(Guid? id)
        {
            var response = await _httpClient.GetStringAsync($"https://localhost:7170/api/Promotions/{id}");
            var PromotionItem = JsonConvert.DeserializeObject<Promotion>(response);
            return PromotionItem;
        }

        public async Task Update(Promotion promotion)
        {
            await _httpClient.PutAsJsonAsync($"https://localhost:7170/api/Promotions/{promotion.Id}", promotion);
        }
    }
}
