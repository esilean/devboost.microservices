using System;

namespace DroneDelivery.Pagamento.Application.Dtos
{
    public class CriarPedidoPagamentoDto
    {

        public string NumeroCartao { get; set; }

        public DateTime VencimentoCartao { get;  set; }

        public int CodigoSeguranca { get;  set; }
    }
}
