using System;

namespace DroneDelivery.Pagamento.Application.Dtos
{
    public class CriarPedidoDto
    {

        public Guid Id { get; set; }

        public double Valor { get; set; }
    }
}
