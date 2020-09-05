using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

namespace DroneDelivery.Infra.Jwt
{
    public class JwtSettings
    {
        public string Audience { get; }
        public string Issuer { get; }
        public int ValidadeEmMinutos { get; }
        public int RefreshTokenValidadeEmMinutos { get; }
        public SigningCredentials SigningCredentials { get; }

        public DateTime IssuedAt => DateTime.UtcNow;
        public DateTime NotBefore => IssuedAt;
        public DateTime DataTokenExpiracao => IssuedAt.AddMinutes(ValidadeEmMinutos);
        public DateTime DataRefreshTokenExpiracao => IssuedAt.AddMinutes(RefreshTokenValidadeEmMinutos);

        public JwtSettings(IConfiguration configuration)
        {
            Issuer = configuration["JwtSettings:Issuer"];
            Audience = configuration["JwtSettings:Audience"];
            ValidadeEmMinutos = Convert.ToInt32(configuration["JwtSettings:ValidadeEmMinutos"]);
            RefreshTokenValidadeEmMinutos = Convert.ToInt32(configuration["JwtSettings:RefreshTokenValidadeEmMinutos"]);
            var signingKey = configuration["JwtSettings:SigningKey"];
            var symmetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey));
            SigningCredentials = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256Signature);
        }
    }
}
