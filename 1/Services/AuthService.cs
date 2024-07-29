using _1.Interfaces;
using _1.DbModels;
using Microsoft.EntityFrameworkCore;
using _1.Responses;
using _1.Requests;
using Newtonsoft.Json.Linq;

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

        public async Task<AuthorizationResponse> Register(RegistrationRequest registrationRequest)
        {
            var salt = BCrypt.Net.BCrypt.GenerateSalt();
            
            User user = new User
            {
                Email = registrationRequest.Email,
                Login = registrationRequest.Login,
                Password = BCrypt.Net.BCrypt.HashPassword(registrationRequest.Password, salt!),
                Salt = salt,
                UserRole = _dbContext.UserRoles.First(r=>r.Role=="Пользователь")
            };
            
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            return _jwtTokenService.GenerateTokens(user);
        }

        public async Task<AuthorizationResponse> Authenticate(AuthorizationRequest authorizationRequest)
        {
            var existingUser = await _dbContext.Users.Where(u => u.Login == authorizationRequest.Login).Include(u=>u.UserRole).FirstOrDefaultAsync();

            if (existingUser == null || existingUser.Password == BCrypt.Net.BCrypt.HashPassword(authorizationRequest.Password, existingUser.Salt!))
                throw new Exception("Неверный логин или пароль");
            else
                return _jwtTokenService.GenerateTokens(existingUser);
        }

        public async Task<AuthorizationResponse> RefreshToken(RefreshTokenRequest refreshTokenRequest)
        {
            User user = await _dbContext.Users.Where(u => u.IdUser == refreshTokenRequest.UserId).Include(u => u.UserRole).FirstOrDefaultAsync();

            return await _jwtTokenService.RefreshAccessToken(refreshTokenRequest, user!);
        }
    }
}
