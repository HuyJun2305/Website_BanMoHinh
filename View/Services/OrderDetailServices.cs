using Data.Models;
using Newtonsoft.Json;
using View.IServices;

namespace View.Services
{
    public class OrderDetailServices : IOrderDetailServices
    {
        private readonly HttpClient _client;

        public OrderDetailServices(HttpClient client)
        {
            _client = client;
        }

        public Task<OrderDetail?> AddOrUpdateOrderDetail(Guid orderId, Guid productId, int quantity)
        {
            throw new NotImplementedException();
        }

        public Task CreateAsync(OrderDetail orderDetail)
        {
            throw new NotImplementedException();
        }

        public Task<OrderDetail?> GetOrderDetailByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<OrderDetail?> GetOrderDetailByOrderAndProductIdAsync(Guid orderId, Guid productId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<OrderDetail>?> GetOrderDetailsByOrderIdAsync(Guid orderId)
        {
            var respone = await _client.GetStringAsync($"https://localhost:7280/api/OrderDetails/GetOrderDetailByOrderId?orderId={orderId}");
            var result = JsonConvert.DeserializeObject<List<OrderDetail>>(respone);
            return result;
        }

        public Task<decimal> GetTotalPriceByOrderIdAsync(Guid orderId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveOrderDetail(Guid orderId, Guid productId, int quantityToRemove)
        {
            throw new NotImplementedException();
        }

        public Task SaveChangesAsync()
        {
            throw new NotImplementedException();
        }
    }
}
