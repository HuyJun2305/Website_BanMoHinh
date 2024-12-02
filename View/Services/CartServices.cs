using Data.Models;
using Newtonsoft.Json;
using System.Net;
using View.IServices;

namespace View.Services
{
	public class CartServices : ICartServices
	{
		private readonly HttpClient _client;

		public CartServices(HttpClient client)
		{
			_client = client;
		}

		public async Task<CartDetail> AddToCart(Guid cartId, Guid productId, int quantity)
		{
			var response = await _client.GetStringAsync($"https://localhost:7280/api/CartDetail/CartDetail/AddToCart?cartId={cartId}&productId={productId}&quantity={quantity}");
			var result = JsonConvert.DeserializeObject<CartDetail>(response);
			return result;
		}

		public async Task CreateCart(Cart cart)
		{
			await _client.PostAsJsonAsync("https://localhost:7280/api/Cart", cart);
		}
		public async Task CreateCartDetails(CartDetail cartDetail)
		{
			var cartDetailsItem = (await GetCartDetailByCartId(cartDetail.CartId))
				.Where(o => o.ProductId == cartDetail.ProductId)
				.FirstOrDefault();

			if (cartDetailsItem != null)
			{
				cartDetailsItem.Quatity += cartDetail.Quatity;

				// Kiểm tra nếu số lượng vượt quá tồn kho thì giới hạn lại bằng số lượng tồn kho
				if (cartDetailsItem.Quatity > cartDetail.Product.Stock)
				{
					cartDetailsItem.Quatity = cartDetail.Product.Stock;
				}

				cartDetailsItem.TotalPrice = cartDetail.Product.Price * cartDetailsItem.Quatity;
				cartDetailsItem.Product = null;
				await _client.PutAsJsonAsync($"https://localhost:7280/api/CartDetail/Update?id={cartDetailsItem.Id}", cartDetailsItem);
			}
			else
			{
				cartDetail.Product = null;
				await _client.PostAsJsonAsync($"https://localhost:7280/api/CartDetail/Create", cartDetail);
			}
		}

		public async Task Delete(Guid id)
		{
			await _client.DeleteAsync($"https://localhost:7280/api/CartDetail/Delete?id={id}");
		}

		public async Task<List<CartDetail>> GetAllCartDetails()
		{
			var response = await _client.GetStringAsync("https://localhost:7280/api/CartDetail/GetAllCartDetails");
			var result = JsonConvert.DeserializeObject<List<CartDetail>>(response);
			return result;
		}

		public async Task<Cart> GetCartAsync(Guid id)
		{
			var respone = await _client.GetStringAsync($"https://localhost:7280/api/CartDetail/GetCartById?id={id}");
			var result = JsonConvert.DeserializeObject<Cart>(respone);
			return result;
		}

		public async Task<Cart> GetCartByUserId(Guid userId)
		{
			try
			{
				var response = await _client.GetStringAsync($"https://localhost:7280/api/Cart/GetCartByUserId?userId={userId}");
				var result = JsonConvert.DeserializeObject<Cart>(response);
				return result;
			}
			catch (HttpRequestException ex)
			{
				if (ex.StatusCode == HttpStatusCode.NotFound)
				{
					return null;
				}
				throw;
			}
		}


		public async Task<List<CartDetail>> GetCartDetailByCartId(Guid cartId)
		{
			try
			{
				var response = await _client.GetStringAsync($"https://localhost:7280/api/CartDetail/GetCartDetailsByCartId?cartId={cartId}");
				var result = JsonConvert.DeserializeObject<List<CartDetail>>(response);

				// Kiểm tra xem kết quả có null hay rỗng không
				return result ?? new List<CartDetail>(); // Trả về danh sách rỗng nếu không có dữ liệu
			}
			catch (HttpRequestException ex)
			{
				Console.WriteLine($"Error fetching cart details: {ex.Message}");
				return new List<CartDetail>();
			}
		}

		public async Task<Cart> GetCartDetailById(Guid id)
		{
			var response = await _client.GetStringAsync($"https://localhost:7280/api/CartDetail/GetCartDetailsById?id={id}");
			var result = JsonConvert.DeserializeObject<Cart>(response);
			return result;
		}

		public async Task<CartDetail> GetCartDetailByProductId(Guid cartId, Guid productId)
		{
			var response = await _client.GetStringAsync($"https://localhost:7280/api/CartDetail/GetCartDetailsById?cartId={cartId}&productId={productId}");
			var result = JsonConvert.DeserializeObject<CartDetail>(response);
			return result;
		}

		public async Task Update(Cart cart, Guid id)
		{
			var cartDetailsItem = await GetCartDetailByCartId(id);
			decimal totalPrice = 0;
			if (cartDetailsItem != null)
			{
				foreach (var item in cartDetailsItem)
				{
					totalPrice += (decimal)item.TotalPrice;
				};
			}
			var cartUser = await GetCartAsync(id);
			cartUser.TotalPrice = totalPrice;
			await _client.PutAsJsonAsync($"https://localhost:7280/api/Cart/{id}", cart);
		}

	

		
	}
}
