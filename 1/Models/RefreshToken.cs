﻿namespace _1.Models
{
    public class RefreshToken
    {
        public string Token { get; set; } = null!;
        public DateTime Expires { get; set; }
        public DateTime Created { get; set; }
    }
}
