using DroneDelivery.Shared.Domain.Core.Commands;
using Flunt.Validations;
using Newtonsoft.Json;

namespace DroneDelivery.Application.Commands.Usuarios
{
    public class LoginUsuarioCommand : Command
    {
        public string Email { get;  }

        public string Password { get; }

        [JsonConstructor]
        public LoginUsuarioCommand(string email, string password)
        {
            Email = email;
            Password = password;
        }

        public void Validate()
        {
            AddNotifications(new Contract()
                .Requires()
                .IsNotNullOrEmpty(Email, nameof(Email), "O Email não pode ser vazio"));

            AddNotifications(new Contract()
                .Requires()
                .IsEmail(Email, nameof(Email), "o Email não é valido"));

            AddNotifications(new Contract()
                .Requires()
                .IsNotNullOrEmpty(Password, nameof(Password), "O Password não pode ser vazio"));
        }
    }
}
