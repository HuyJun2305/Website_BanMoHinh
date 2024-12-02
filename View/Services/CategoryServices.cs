using Data.Models;
using Newtonsoft.Json;
using View.IServices;

namespace View.Services
{
	public class CategoryServices : ICategoryServices
	{
		private readonly HttpClient _httpClient;

		public CategoryServices(HttpClient httpClient)
		{
			_httpClient = httpClient;
		}

		public async Task Create(Category category)
		{
			await _httpClient.PostAsJsonAsync("https://localhost:7280/api/Category", category);
		}

		public async Task Delete(Guid id)
		{
			await _httpClient.DeleteAsync($"https://localhost:7280/api/Category/{id}");
		}

		public async Task<IEnumerable<Category>> GetAllCategories()
		{
			var response = await _httpClient.GetStringAsync("https://localhost:7280/api/Category");
			IEnumerable<Category> categories = JsonConvert.DeserializeObject<IEnumerable<Category>>(response);
			return categories;
		}

		public async Task<Category> GetCategoryById(Guid id)
		{
            var response = await _httpClient.GetStringAsync($"https://localhost:7280/api/Category/{id}");
            Category category = JsonConvert.DeserializeObject<Category>(response);
            return category;
        }

        public async Task Update(Category category)
		{
			await _httpClient.PutAsJsonAsync($"https://localhost:7280/api/Category/{category.Id}", category);
		}

		
	}
}
