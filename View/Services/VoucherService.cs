using Data.Models;
using Newtonsoft.Json;
using View.IServices;

namespace View.Services
{
    public class VoucherService : IVoucherService
    {
        private readonly HttpClient _httpClient;

        public VoucherService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task Create(Voucher voucher)
        {
            await _httpClient.PostAsJsonAsync("https://localhost:7280/api/Voucher", voucher);
        }

        public async Task Delete(Guid id)
        {
            await _httpClient.DeleteAsync($"https://localhost:7280/api/Voucher/{id}");
        }

        public async Task<IEnumerable<Voucher>?> GetAllVouchers()
        {
            var response = await _httpClient.GetStringAsync("https://localhost:7280/api/Voucher");
            var listVoucher = JsonConvert.DeserializeObject<List<Voucher>>(response);
            return listVoucher;
        }

        public async Task<Voucher?> GetVoucherById(Guid id)
        {
            var response = await _httpClient.GetStringAsync($"https://localhost:7280/api/Voucher/{id}");
            var voucherItem = JsonConvert.DeserializeObject<Voucher>(response);
            return voucherItem;
        }

        public async Task Update(Voucher voucher)
        {
            await _httpClient.PutAsJsonAsync($"https://localhost:7280/api/Voucher/{voucher.Id}", voucher);
        }
    }
}
