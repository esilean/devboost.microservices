namespace DroneDelivery.Api.Tests.Dtos.Token
{
    public class JsonWebTokenTestDto
    {
        public string Email { get; set; }
        public TokenTestDto Jwt { get; set; }
    }

    public class TokenTestDto
    {
        public string Token { get; set; }

        public RefreshTokenTestDto RefreshToken { get; set; }
    }

    public class RefreshTokenTestDto
    {
        public string Token { get; set; }
    }
}
