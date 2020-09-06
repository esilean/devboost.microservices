using DroneDelivery.Shared.Domain.Core.Enums;
using System;

namespace DroneDelivery.Shared.Domain.Core.Events.Pedidos
{
    public class PedidoStatusAtualizadoEvent : Event
    {
        public Guid Id { get; private set; }
        public PedidoStatus Status { get; private set; }

        public PedidoStatusAtualizadoEvent(Guid id, PedidoStatus status)
        {
            Id = id;
            Status = status;
        }
    }
}
