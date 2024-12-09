using Data.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NuGet.Common;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using View.IServices;
using View.ViewModels;

namespace View.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        //private readonly IHttpClientFactory _factory;

        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;
        private string? _jwtCache;
        private static LoginResponse Login;

        public event Action<string?>? LoginChange;
        public AuthenticationService(/*IHttpClientFactory factory*/HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            //_factory = factory;
        }
        public async ValueTask<string> GetJwtAsync()
        {
            if (string.IsNullOrEmpty(_jwtCache))
                _jwtCache = Login.JwtToken;
            return _jwtCache;
        }
        public async Task LogoutAsync()
        {
            //await _factory.CreateClient("ServerApi").DeleteAsync("api/Authentication");
            string requestURL = "https://localhost:7280/api/Authentication";

            // Gửi request tới API
            var response = await _httpClient.DeleteAsync(requestURL);

            Login = null;

            _jwtCache = null;

            LoginChange?.Invoke(null);

        }

        private static string GetUsername(string token)
        {
            var jwt = new JwtSecurityToken(token);
            
            return jwt.Claims.First(c => c.Type == ClaimTypes.Name).Value;
        }

		public async ValueTask<string> LoginAsync(ViewModels.Sign_In_Up_ViewModel model)
		{
			string requestURL = "https://localhost:7280/api/Authentication/Login";

			// Gửi request tới API
			var response = await _httpClient.PostAsJsonAsync(requestURL, model);

			if (!response.IsSuccessStatusCode)
			{
				// Log thêm chi tiết về lỗi
				var errorContent = await response.Content.ReadAsStringAsync();
				throw new UnauthorizedAccessException($"Đăng nhập thất bại! Lỗi: {errorContent}");
			}

			var content = await response.Content.ReadFromJsonAsync<LoginResponse>();

			if (content == null) throw new InvalidDataException();

			_jwtCache = content.JwtToken;
			Login = content;

			return Login.JwtToken;
		}
        public async Task<bool> Register(ViewModels.Sign_In_Up_ViewModel model)
        {
            string requestURL = "https://localhost:7280/api/Authentication/Register";

            // Gửi request tới API
            var response = await _httpClient.PostAsJsonAsync(requestURL, model);
            if (!response.IsSuccessStatusCode)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> RefreshAsync()
        {
            var model = new RefreshModel
            {
                AccessToken = Login.JwtToken,
                RefreshToken = Login.RefreshToken
            };
            string requestURL = "https://localhost:7280/api/Authentication/Refresh";

            // Gửi request tới API
            var response = await _httpClient.PostAsJsonAsync(requestURL, model);

            if (!response.IsSuccessStatusCode)
            {
                await LogoutAsync();

                return false;
            }

            var content = await response.Content.ReadFromJsonAsync<LoginResponse>();

            if (content == null)
                throw new InvalidDataException();

            Login = content;
            _jwtCache = content.JwtToken;
            return true;

        }
    }
}
