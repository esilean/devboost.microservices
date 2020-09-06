using System;

namespace DroneDelivery.Shared.Domain.Core.Events.Pedidos
{
    public class PedidoCriadoEvent : Event
    {
        public Guid Id { get; private set; }

        public double Valor { get; private set; }

        public PedidoCriadoEvent(Guid id, double valor)
        {
            Id = id;
            Valor = valor;
        }
    }
}
