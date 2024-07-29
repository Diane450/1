namespace _1.Responses
{
    public class AuthorizationResponse
    {
        public string AccessToken { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;
        public DateTime RefreshTokenExpires { get; set; }
        public int UserId { get; set; }
    }
}
