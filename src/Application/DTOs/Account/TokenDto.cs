using System;

namespace Application.DTOs.Account
{
    public class TokenDto
    {
        public string Token { get; set; }
        public Guid RefreshToken { get; set; }
    }
}
