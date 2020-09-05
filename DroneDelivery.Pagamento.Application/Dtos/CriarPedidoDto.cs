using System;

namespace DroneDelivery.Pagamento.Application.Dtos
{
    public class CriarPedidoDto
    {

        public Guid PedidoId { get; set; }

        public double Valor { get; set; }
    }
}
