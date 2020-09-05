using System;

namespace DroneDelivery.Pagamento.Application.Dtos
{
    public class CriarPedidoPagamentoDto
    {
        public Guid PedidoId { get; set; }

        public double Valor { get; set; }

        public string NumeroCartao { get; set; }

        public DateTime VencimentoCartao { get;  set; }

        public int CodigoSeguranca { get;  set; }
    }
}
