using System;

namespace DroneDelivery.Application.Dtos.Token
{
    public class RefreshTokenDto
    {
        public string Email { get; set; }
        public string Token { get; set; }
        public DateTime DataExpiracao { get; set; }
    }
}
