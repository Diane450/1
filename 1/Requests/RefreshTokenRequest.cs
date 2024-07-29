namespace _1.Requests
{
    public class RefreshTokenRequest
    {
        public string Token { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;
        public DateTime Expires{ get; set; }
        public int UserId { get; set; }

    }
}
