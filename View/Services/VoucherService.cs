using Data.Models;
using Newtonsoft.Json;
using View.Iservices;

namespace View.Services
{
    public class VoucherService : IVoucherService
    {
        private readonly HttpClient _httpClient;

        public VoucherService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }



        // Create Voucher
        public async Task Create(Voucher voucher)
        {
            var response = await _httpClient.PostAsJsonAsync("https://localhost:7170/api/Voucher", voucher);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Failed to create voucher. Status Code: {response.StatusCode}, Error: {errorContent}");
            }
        }

        // Delete Voucher
        public async Task Delete(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"https://localhost:7170/api/Voucher/{id}");

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Failed to delete voucher. Status Code: {response.StatusCode}, Error: {errorContent}");
            }
        }

        // Get All Vouchers
        public async Task<IEnumerable<Voucher>?> GetAllVouchers()
        {
            var response = await _httpClient.GetAsync("https://localhost:7170/api/Voucher");

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Failed to retrieve vouchers. Status Code: {response.StatusCode}, Error: {errorContent}");
            }

            var responseString = await response.Content.ReadAsStringAsync();
            var vouchers = JsonConvert.DeserializeObject<IEnumerable<Voucher>>(responseString);
            return vouchers;
        }

        // Get Voucher by ID
        public async Task<Voucher?> GetVoucherById(Guid id)
        {
            var response = await _httpClient.GetAsync($"https://localhost:7170/api/Voucher/{id}");

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Failed to retrieve voucher. Status Code: {response.StatusCode}, Error: {errorContent}");
            }

            var responseString = await response.Content.ReadAsStringAsync();
            var voucher = JsonConvert.DeserializeObject<Voucher>(responseString);
            return voucher;
        }

        // Update Voucher
        public async Task Update(Voucher voucher)
        {
            var response = await _httpClient.PutAsJsonAsync($"https://localhost:7170/api/Voucher/{voucher.Id}", voucher);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Failed to update voucher. Status Code: {response.StatusCode}, Error: {errorContent}");
            }
        }
    }
}
