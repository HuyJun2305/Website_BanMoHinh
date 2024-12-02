using Data.Models;
using Newtonsoft.Json;
using View.IServices;

namespace View.Servicecs
{
    public class AddresServices : IAddresServices
    {
        private readonly HttpClient _httpClient;
        public AddresServices(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task Create(Address address)
        {
            await _httpClient.PostAsJsonAsync("https://localhost:7280/api/Addres", address);
        }

        public async Task Delete(Guid id)
        {
            await _httpClient.DeleteAsync($"https://localhost:7280/api/Addres/{id}");
        }

        public async Task<IEnumerable<Address>> GetAllBrands()
        {
            var response = await _httpClient.GetFromJsonAsync<IEnumerable<Address>>("https://localhost:7280/api/Addres");
            return response;
        }

        public async Task<Address> GetBrandById(Guid id)
        {
            var response = await _httpClient.GetFromJsonAsync<Address>($"https://localhost:7280/api/Addres/{id}");
            return response;
        }

        public async Task Update(Address address)
        {
            await _httpClient.PutAsJsonAsync($"https://localhost:7280/api/Addres/{address.Id}", address);
        }
    }
}
