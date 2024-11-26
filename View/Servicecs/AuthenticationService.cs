using Data.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Security.Claims;
using View.IServices;

namespace View.Servicecs
{
    public class AuthenticationService : IAuthenticationService
    {
        //private readonly IHttpClientFactory _factory;
        private readonly HttpClient _httpClient;
        private string? _jwtCache;
        private static LoginResponse Login;
        public AuthenticationService(/*IHttpClientFactory factory*/HttpClient httpClient)
        {
            _httpClient = httpClient;
            //_factory = factory;
        }
        public async ValueTask<string> GetJwtAsync()
        {
            if (string.IsNullOrEmpty(_jwtCache))
                _jwtCache =  Login.JwtToken;
            return _jwtCache;
        }
        public async Task LogoutAsync()
        {
            //await _factory.CreateClient("ServerApi").DeleteAsync("api/Authentication");
            string requestURL = "https://localhost:7280/api/Authentication";

            // Gửi request tới API
            var response = await _httpClient.DeleteAsync(requestURL);
            Login = null;

        }

        private static string GetUsername(string token)
        {
            var jwt = new JwtSecurityToken(token);
            return jwt.Claims.First(c => c.Type == ClaimTypes.Name).Value;
        }
        public async Task<LoginResponse> LoginAsync(LoginModel model)
        {
            //var response = await _factory.CreateClient("ServerApi").PostAsync("api/Authentication/Login", JsonContent.Create(model));
            string requestURL = "https://localhost:7280/api/Authentication/Login";

            // Gửi request tới API
            var response = await _httpClient.PostAsJsonAsync(requestURL, model);

            if (!response.IsSuccessStatusCode)
                throw new UnauthorizedAccessException("Đăng nhập thất bại!");

            var content = await response.Content.ReadFromJsonAsync<LoginResponse>();

            if (content == null) throw new InvalidDataException();
            else
            {
                _jwtCache = content.JwtToken;
                Login = content;
            }


            return content;
        }
    }
}
