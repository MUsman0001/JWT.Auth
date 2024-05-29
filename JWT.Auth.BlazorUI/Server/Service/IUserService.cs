using JWT.Auth.BlazorUI.Shared.registration;
using JWT.Auth.BlazorUI.Shared.Models;
using JWT.Auth.BlazorUI.Shared.login;
using JWT.Auth.BlazorUI.Shared.tokens;

namespace JWT.Auth.BlazorUI.Server.Service
{
    public interface IUserService
    {
        Task<(bool IsUserRegistered, string Message)> RegisterNewUser(UserRegistrationDto userRegistration);
        Task<(bool IsLoginSucess, JWTTokenResponseDto TokenResponse)> LoginAsync(LoginDto loginpayload);
        //Task LoginaAync(LoginDto payload);
    }
}
