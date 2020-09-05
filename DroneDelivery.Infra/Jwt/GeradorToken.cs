using DroneDelivery.Application.Dtos.Token;
using DroneDelivery.Application.Interfaces;
using DroneDelivery.Domain.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace DroneDelivery.Infra.Jwt
{
    public class GeradorToken : IGeradorToken
    {
        private readonly JwtSettings _settings;

        public GeradorToken(JwtSettings settings)
        {
            _settings = settings;
        }

        private static ClaimsIdentity ObterClaimsIdentity(Usuario usuario)
        {
            var identity = new ClaimsIdentity
            (
                new[] {
                    new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                    new Claim(ClaimTypes.Name, usuario.Nome),
                    new Claim(ClaimTypes.Email, usuario.Email),
                    new Claim(ClaimTypes.Role, usuario.Role.ToString())
                }
            );

            return identity;
        }

        public JsonWebTokenDto GerarToken(Usuario usuario)
        {
            var identity = ObterClaimsIdentity(usuario);
            var handler = new JwtSecurityTokenHandler();
            var securityToken = handler.CreateToken(new SecurityTokenDescriptor
            {
                Subject = identity,
                Issuer = _settings.Issuer,
                Audience = _settings.Audience,
                IssuedAt = _settings.IssuedAt,
                NotBefore = _settings.NotBefore,
                Expires = _settings.DataTokenExpiracao,
                SigningCredentials = _settings.SigningCredentials
            });

            var accessToken = handler.WriteToken(securityToken);

            return new JsonWebTokenDto
            {
                Token = accessToken,
                RefreshToken = GerarRefreshToken(usuario),
                ExpiraEmSegundos = (long)TimeSpan.FromMinutes(_settings.ValidadeEmMinutos).TotalSeconds
            };
        }

        public RefreshTokenDto GerarRefreshToken(Usuario usuario)
        {
            var refreshToken = new RefreshTokenDto
            {
                Email = usuario.Email,
                DataExpiracao = _settings.DataRefreshTokenExpiracao
            };

            string token;
            var randomNumber = new byte[32];

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                token = Convert.ToBase64String(randomNumber);
            }

            refreshToken.Token = token.Replace("+", string.Empty)
                .Replace("=", string.Empty)
                .Replace("/", string.Empty);

            return refreshToken;
        }
    }
}
