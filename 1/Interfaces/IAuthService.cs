namespace _1.Interfaces
{
    public interface IAuthService
    {
        Task<string> Authenticate(string username, string password);
        Task<string> RefreshToken(string token);
        Task RevokeToken(string token);
    }
}
