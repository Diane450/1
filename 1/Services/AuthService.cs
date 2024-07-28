using _1.Interfaces;
using _1.Models;
using Microsoft.EntityFrameworkCore;

namespace _1.Services
{
    public class AuthService : IAuthService
    {

        private readonly Ispr2438IbragimovaDm1Context _dbContext;
        private readonly IJwtTokenService _jwtTokenService;

        public AuthService(Ispr2438IbragimovaDm1Context dbContext, IJwtTokenService jwtTokenService)
        {
            _dbContext = dbContext;
            _jwtTokenService = jwtTokenService;
        }

        public async Task<string> Authenticate(string login, string password)
        {
            var user = await _dbContext.Users.SingleOrDefaultAsync(u => u.Login == login);

            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                throw new Exception("Username or password is incorrect");
            }
            return null;
            //else
            //{
            //    var token = _jwtTokenService.GenerateToken(user);
            //    var refreshToken = _jwtTokenService.GenerateRefreshToken();
            //    return new 
            //}

        }

        public Task<string> RefreshToken(string token)
        {
            throw new NotImplementedException();
        }

        public Task RevokeToken(string token)
        {
            throw new NotImplementedException();
        }
    }
}
