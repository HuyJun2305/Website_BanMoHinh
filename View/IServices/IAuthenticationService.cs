using Data.Authentication;

namespace View.IServices
{
    public interface IAuthenticationService
    {
        ValueTask<string> GetJwtAsync();
        Task<LoginResponse> LoginAsync(LoginModel model);
        Task LogoutAsync();
    }
}