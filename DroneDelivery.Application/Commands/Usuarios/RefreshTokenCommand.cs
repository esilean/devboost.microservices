using DroneDelivery.Shared.Domain.Core.Commands;
using Flunt.Validations;
using Newtonsoft.Json;

namespace DroneDelivery.Application.Commands.Usuarios
{
    public class RefreshTokenCommand : Command
    {
        public string Email { get; }
        public string Token { get; }
        public string RefreshToken { get; }

        [JsonConstructor]
        public RefreshTokenCommand(string email, string token, string refreshToken)
        {
            Email = email;
            Token = token;
            RefreshToken = refreshToken;
        }

        public void Validate()
        {
            AddNotifications(new Contract()
                .Requires()
                .IsNotNullOrEmpty(Email, nameof(Email), "O Email não pode ser vazio"));

            AddNotifications(new Contract()
                .Requires()
                .IsEmail(Email, nameof(Email), "O Email não é valido"));

            AddNotifications(new Contract()
                .Requires()
                .IsNotNullOrEmpty(Token, nameof(Token), "O Token não pode ser vazio"));

            AddNotifications(new Contract()
                .Requires()
                .IsNotNullOrEmpty(RefreshToken, nameof(RefreshToken), "O RefreshToken não pode ser vazio"));
        }
    }


}
