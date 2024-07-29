using _1.DbModels;
using _1.Models;
using _1.Requests;
using _1.Responses;

namespace _1.Interfaces
{
    public interface IJwtTokenService
    {
        string GenerateAccessToken(User user);
        RefreshToken GenerateRefreshToken();
        Task<AuthorizationResponse> RefreshAccessToken(RefreshTokenRequest refreshTokenRequest, User user);
        AuthorizationResponse GenerateTokens(User user);

    }
}
