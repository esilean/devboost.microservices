using DroneDelivery.Domain.Enum;
using DroneDelivery.Shared.Domain.Core.Domain;
using System;

namespace DroneDelivery.Domain.Models
{
    public class Usuario : Entity, IAggregateRoot
    {

        public string Nome { get; private set; }
        public string Email { get; private set; }
        public string PasswordHash { get; private set; }
        public string RefreshToken { get; private set; }
        public DateTime RefreshTokenExpiracao { get; private set; }

        public double Latitude { get; private set; }
        public double Longitude { get; private set; }

        public UsuarioRole Role { get; private set; }

        private Usuario(string nome, string email, double latitude, double longitude, UsuarioRole role)
        {
            Nome = nome;
            Email = email;
            Latitude = latitude;
            Longitude = longitude;
            Role = role;
        }

        public static Usuario Criar(string nome, string email, double latitude, double longitude, UsuarioRole role)
        {
            return new Usuario(nome, email, latitude, longitude, role);
        }

        public void AdicionarPassword(string password)
        {
            PasswordHash = password;
        }

        public void AdicionarRefreshToken(string refreshToken, DateTime refreshTokenExpiracao)
        {
            RefreshToken = refreshToken;
            RefreshTokenExpiracao = refreshTokenExpiracao;
        }

    }
}
