using DroneDelivery.Shared.Domain.Core.Commands;
using Flunt.Validations;
using Newtonsoft.Json;

namespace DroneDelivery.Application.Commands.Usuarios
{
    public class CriarUsuarioCommand : Command
    {
        public string Email { get; }
        public string Nome { get; }
        public double Latitude { get; }
        public double Longitude { get; }
        public string Password { get; }

        [JsonConstructor]
        public CriarUsuarioCommand(string email, string nome, double latitude, double longitude, string password)
        {
            Email = email;
            Nome = nome;
            Latitude = latitude;
            Longitude = longitude;
            Password = password;
        }

        public void Validate()
        {
            AddNotifications(new Contract()
                .Requires()
                .IsNotNullOrEmpty(Nome, nameof(Nome), "O Nome não pode ser vazio"));

            AddNotifications(new Contract()
                .Requires()
                .IsNotNullOrEmpty(Password, nameof(Password), "O Password não pode ser vazio"));

            AddNotifications(new Contract()
                .Requires()
                .IsNotNullOrEmpty(Email, nameof(Email), "O Email não pode ser vazio"));

            AddNotifications(new Contract()
                .Requires()
                .IsEmail(Email, nameof(Email), "O Email não é valido"));

            AddNotifications(new Contract()
                .Requires()
                .AreNotEquals(Latitude, 0, nameof(Latitude), "A Latitude não pode ser vazia"));

            AddNotifications(new Contract()
                .Requires()
                .AreNotEquals(Longitude, 0, nameof(Longitude), "A Longitude não pode ser vazia"));
        }
    }
}
