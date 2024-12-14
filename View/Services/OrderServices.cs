using Data.Models;
using Newtonsoft.Json;
using System.Net.Http;
using View.IServices;

namespace View.Services
{
	public class OrderServices : IOrderServices
	{
		private readonly HttpClient _client;

		public OrderServices(HttpClient client)
		{
			_client = client;
		}

		public async Task Create(Order order)
		{
            await _client.PostAsJsonAsync("https://localhost:7280/api/Orders", order);
        }

        public async Task<Order> CreateByStaff(Guid staffId, Guid? customerId = null, Guid? voucherid = null)
		{
			var response = await _client.GetStringAsync($"https://localhost:7280/api/Orders/create-by-staff?=staffId={staffId}=customerId={customerId}&voucherId={voucherid}");
			var result = JsonConvert.DeserializeObject<Order>(response);
			return result;

		}

		public async Task Delete(Guid id)
		{
			 await _client.GetStringAsync($"https://localhost:7280/api/Orders/DeleteOrderById?=id={id}");

		}

		public async Task<IEnumerable<Order>> GetAllOrder()
		{
			var response = await _client.GetStringAsync("https://localhost:7280/api/Orders/GetAllOrder");
			var result = JsonConvert.DeserializeObject<List<Order>>(response);
			return result;
		}

        public async Task<IEnumerable<Order>> GetAllOrderStatus0()
        {
            var response = await _client.GetStringAsync("https://localhost:7280/api/Orders/GetOrderStatus0");
            var result = JsonConvert.DeserializeObject<IEnumerable<Order>>(response);
            return result;
        }

        public async Task<Order> GetOrderById(Guid id)
		{
			var response = await _client.GetStringAsync($"https://localhost:7280/api/Orders/GetOrderById?id={id}");
			var result = JsonConvert.DeserializeObject<Order>(response);
			return result;
		}

        public async Task<IEnumerable<Order>> GetOrdersByCustomerId(Guid customerId)
        {
            var response = await _client.GetStringAsync($"https://localhost:7280/api/Orders/GetOrderByCustomerId?customerId={customerId}");
            var result = JsonConvert.DeserializeObject<IEnumerable<Order>>(response);
            return result;
        }

        public async Task<IEnumerable<Order>> GetOrdersByCustomerIdAndStatus(Guid customerId, OrderStatus status)
        {
            var response = await _client.GetStringAsync($"https://localhost:7280/api/Orders/GetOrderByCustomerId?customerId={customerId}&status{status}");
            var result = JsonConvert.DeserializeObject<IEnumerable<Order>>(response);
            return result;
        }

        public async Task<IEnumerable<Order>> GetOrdersByStatus(OrderStatus status)
        {
            var response = await _client.GetStringAsync($"https://localhost:7280/api/Orders/GetOrdersByStatus?status={status}");
            var result = JsonConvert.DeserializeObject<IEnumerable<Order>>(response);
            return result;
        }

        public Task Update(Order order)
		{
			throw new NotImplementedException();
		}

    }
}
