using DroneDelivery.Shared.Domain.Core.Commands;
using Flunt.Validations;
using Newtonsoft.Json;
using System;

namespace DroneDelivery.Pagamento.Application.Commands.Pedidos
{
    public class CriarPedidoCommand : Command
    {
        public Guid Id { get; }

        public double Valor { get; }

        [JsonConstructor]
        public CriarPedidoCommand(Guid id, double valor)
        {
            Id = id;
            Valor = valor;
        }

        public void Validate()
        {

            AddNotifications(new Contract()
                .Requires()
                .IsNotEmpty(Id, nameof(Id), "O Id do Pedido não pode ser vazio"));

            AddNotifications(new Contract()
                .Requires()
                .IsGreaterThan(Valor, 0, nameof(Valor), "O Valor tem que ser maior que zero"));
        }
    }
}
