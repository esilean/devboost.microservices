using DroneDelivery.Pagamento.Domain.Enums;
using System;

namespace DroneDelivery.Pagamento.Application.Dtos
{
    public class CriarRepostaPagamentoDto
    {
        public Guid Id { get; set; }
        public PedidoStatus Status { get; set; }
    }
}
