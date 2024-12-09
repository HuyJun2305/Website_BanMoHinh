using Data.Authentication;
using System.IdentityModel.Tokens.Jwt;
using View.ViewModels;

namespace View.IServices
{
    public interface IAuthenticationService
    {
        event Action<string?>? LoginChange;

        ValueTask<string> GetJwtAsync();
        ValueTask<string> LoginAsync(ViewModels.Sign_In_Up_ViewModel model);
        Task<bool> Register(ViewModels.Sign_In_Up_ViewModel model);
        Task LogoutAsync();
        Task<bool> RefreshAsync();
    }
}