using Data.Models;
using Newtonsoft.Json;
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

		public Task Create(Order order)
		{
			throw new NotImplementedException();
		}

		public async Task<Order> CreateByStaff(Guid staffId, Guid? customerId = null, Guid? voucherid = null)
		{
			var response = await _client.GetStringAsync($"https://localhost:7280/api/Orders/create-by-staff?=staffId={staffId}=customerId={customerId}&voucherId={voucherid}");
			var result = JsonConvert.DeserializeObject<Order>(response);
			return result;

		}

		public async Task Delete(Guid id)
		{
			 await _client.GetStringAsync($"https://localhost:7280/api/Orders/DeleteOrderById?={id}");

		}

		public async Task<List<Order>> GetAllOrder()
		{
			var response = await _client.GetStringAsync("https://localhost:7280/api/Orders/GetAllOrder");
			var result = JsonConvert.DeserializeObject<List<Order>>(response);
			return result;
		}

		public async Task<Order> GetOrderById(Guid id)
		{
			var response = await _client.GetStringAsync($"https://localhost:7280/api/Orders/GetOrderById?id={id}");
			var result = JsonConvert.DeserializeObject<Order>(response);
			return result;
		}

		public Task Update(Order order)
		{
			throw new NotImplementedException();
		}
	}
}
