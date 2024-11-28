using Data.Authentication;

namespace View.IServices
{
    public interface IAuthenticationService
    {
        ValueTask<string> GetJwtAsync();
        ValueTask<string> LoginAsync(LoginModel model);
        Task LogoutAsync();
        Task<bool> RefreshAsync();
    }
}