using _1.DbModels;
using _1.Requests;
using _1.Responses;

namespace _1.Interfaces
{
    public interface IAuthService
    {
        Task<AuthorizationResponse> Register(RegistrationRequest registrationRequest);
        Task<AuthorizationResponse> Authenticate(AuthorizationRequest authorizationRequest);
        //Task RevokeToken(string token);
        Task<AuthorizationResponse> RefreshToken(RefreshTokenRequest refreshTokenRequest);
    }
}
