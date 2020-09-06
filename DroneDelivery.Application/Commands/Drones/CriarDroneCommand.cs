using DroneDelivery.Shared.Domain.Core.Commands;
using Flunt.Validations;
using Newtonsoft.Json;

namespace DroneDelivery.Application.Commands.Drones
{
    public class CriarDroneCommand : Command
    {
        public double Capacidade { get; }

        public double Velocidade { get; }

        public double Autonomia { get; }

        public double Carga { get; }

        [JsonConstructor]
        public CriarDroneCommand(double capacidade, double velocidade, double autonomia, double carga)
        {
            Capacidade = capacidade;
            Velocidade = velocidade;
            Autonomia = autonomia;
            Carga = carga;
        }

        public void Validate()
        {
            AddNotifications(new Contract()
                .Requires()
                .IsGreaterThan(Capacidade, 0, nameof(Capacidade), "A Capacidade tem que ser maior que zero"));

            AddNotifications(new Contract()
                .Requires()
                .IsGreaterThan(Velocidade, 0, nameof(Velocidade), "A Velocidade tem que ser maior que zero"));

            AddNotifications(new Contract()
                .Requires()
                .IsGreaterThan(Autonomia, 0, nameof(Autonomia), "A Autonomia tem que ser maior que zero"));

            AddNotifications(new Contract()
                .Requires()
                .IsGreaterThan(Carga, 0, nameof(Carga), "A Carga tem que ser maior que zero"));
        }
    }
}
