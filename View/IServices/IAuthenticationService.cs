using Data.Authentication;
using System.IdentityModel.Tokens.Jwt;

namespace View.IServices
{
    public interface IAuthenticationService
    {
        event Action<string?>? LoginChange;

        ValueTask<string> GetJwtAsync();
        ValueTask<string> LoginAsync(LoginModel model);
        Task LogoutAsync();
        Task<bool> RefreshAsync();
    }
}