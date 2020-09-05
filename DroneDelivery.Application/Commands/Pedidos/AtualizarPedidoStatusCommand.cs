using DroneDelivery.Domain.Core.Commands;
using DroneDelivery.Domain.Enum;
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
                .IsNotEmpty(Id, nameof(Id), "O PedidoId não pode ser vazio"));
        }
    }
}
