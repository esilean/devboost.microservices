using DroneDelivery.Shared.Domain.Core.Commands;
using DroneDelivery.Shared.Domain.Core.Enums;
using Flunt.Validations;
using Newtonsoft.Json;
using System;

namespace DroneDelivery.Application.Commands.Pedidos
{
    public class AtualizarPedidoStatusCommand : Command
    {
        public Guid Id { get; }
        public PedidoStatus Status { get; }

        [JsonConstructor]
        public AtualizarPedidoStatusCommand(Guid id, PedidoStatus status)
        {
            Id = id;
            Status = status;
        }

        public void Validate()
        {
            AddNotifications(new Contract()
                .Requires()
                .IsNotEmpty(Id, nameof(Id), "O Id do Pedido não pode ser vazio"));
        }
    }
}
