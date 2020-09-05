using DroneDelivery.Domain.Core.Commands;
using DroneDelivery.Domain.Enum;
using Flunt.Validations;
using System;

namespace DroneDelivery.Application.Commands.Pedidos
{
    public class AtualizarPedidoStatusCommand : Command
    {
        public Guid PedidoId { get; set; }
        public PedidoStatus Status { get; set; }

        public void Validate()
        {

            AddNotifications(new Contract()
                .Requires()
                .IsNotEmpty(PedidoId, nameof(PedidoId), "O PedidoId não pode ser vazio"));


        }
    }
}
