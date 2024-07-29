using _1.Interfaces;
using _1.DbModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using _1.Models;
using _1.Responses;
using _1.Requests;
using Newtonsoft.Json.Linq;

namespace _1.Services
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly IConfiguration _configuration;

        public JwtTokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateAccessToken(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Login),
                new Claim(ClaimTypes.Role, user.UserRole.Role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public RefreshToken GenerateRefreshToken()
        {
            return new RefreshToken
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Expires = DateTime.UtcNow.AddDays(7),
                Created = DateTime.UtcNow
            };
        }

        public async Task<AuthorizationResponse> RefreshAccessToken(RefreshTokenRequest refreshTokenRequest, User user)
        {
            if (refreshTokenRequest.Expires < DateTime.Now)
                throw new Exception("RefreshToken просрочен");
            else
            {
                return new AuthorizationResponse
                {
                    RefreshToken = refreshTokenRequest.RefreshToken,
                    RefreshTokenExpires = refreshTokenRequest.Expires,
                    AccessToken = GenerateAccessToken(user)
                };
            }
        }

        public AuthorizationResponse GenerateTokens(User user)
        {
            var token = GenerateAccessToken(user);
            var refreshToken = GenerateRefreshToken();
            return new AuthorizationResponse
            {
                AccessToken = token,
                RefreshToken = refreshToken.Token,
                RefreshTokenExpires = refreshToken.Expires,
                UserId = user.IdUser
            };
        }
    }
}
