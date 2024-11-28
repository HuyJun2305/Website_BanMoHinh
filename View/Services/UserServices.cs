using Data.DTO;
using Data.Models;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System.Net.Http.Json;
using View.IServices;

namespace View.Servicecs
{
    public class UserServices : IUserServices
    {
        private readonly HttpClient _httpClient;
        public UserServices(IHttpClientFactory factory)
        {
            _httpClient = factory.CreateClient("ServerApi");
        }
        //public UserServices()
        //{
            
        //}
        public async Task Create(UserData user1)
        {

            await _httpClient.PostAsJsonAsync("api/User", user1);
        }

        public async Task Delete(string id)
        {
            await _httpClient.DeleteAsync($"api/User/{id}");
        }
        public async Task<IEnumerable<string>?> GetAllRoles(string username)
        {
            var response = await _httpClient.GetFromJsonAsync<IEnumerable<string>>($"api/User/GetRoles/{username}");
            return response;
        }
        public async Task<IEnumerable<ApplicationUser>?> GetAllUser()
        {
            var response = await _httpClient.GetFromJsonAsync<IEnumerable<ApplicationUser>>("api/User");
            return response;
        }

        public Task<List<ApplicationUser>> GetFilteredUser(Guid? searchQuery = null, Guid? sizeId = null, Guid? imageId = null, Guid? brandId = null, Guid? materialId = null)
        {
            throw new NotImplementedException();
        }

        public async Task<ApplicationUser?> GetUserById(string id)
        {
            //var response = await _httpClient.GetStringAsync($"https://localhost:7280/api/Products/{id}");
            var response = await _httpClient.GetFromJsonAsync<ApplicationUser>($"api/User/{id}");
            return response;
        }

        public async Task Update(UserData user, string id)
        {
            await _httpClient.PutAsJsonAsync($"api/User/{id}", user);
        }
    }
}
