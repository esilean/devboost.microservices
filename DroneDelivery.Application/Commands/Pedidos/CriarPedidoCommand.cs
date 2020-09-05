using DroneDelivery.Domain.Core.Commands;
using Flunt.Validations;
using Newtonsoft.Json;
using System;

namespace DroneDelivery.Application.Commands.Pedidos
{
    public class CriarPedidoCommand : Command
    {
        public double Peso { get; }

        public double Valor { get; }

    
        [JsonConstructor]
        public CriarPedidoCommand(double peso, double valor)
        {
            Peso = peso;
            Valor = valor;
        }

        public void Validate()
        {

            AddNotifications(new Contract()
                .Requires()
                .IsGreaterThan(Peso, 0, nameof(Peso), "O Peso tem que ser maior que zero"));

            AddNotifications(new Contract()
                .Requires()
                .IsGreaterThan(Valor, 0, nameof(Valor), "O Valor tem que ser maior que zero"));

            AddNotifications(new Contract()
                .Requires()
                .IsLowerOrEqualsThan(Peso, Utility.Utils.CAPACIDADE_MAXIMA_GRAMAS, nameof(Peso), $"O Peso tem que ser menor ou igual a {Utility.Utils.CAPACIDADE_MAXIMA_GRAMAS / 1000} KGs"));

        }
    }
}
