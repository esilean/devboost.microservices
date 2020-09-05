using DroneDelivery.Pagamento.Domain.Enums;
using System;

namespace DroneDelivery.Pagamento.Domain.Models
{
    public class PedidoPagamento
    {

        public Guid Id { get; private set; }

        public Guid PedidoId { get; private set; }

        public double Valor { get; private set; }

        public string NumeroCartao { get; private set; }

        public DateTime VencimentoCartao { get; private set; }

        public int CodigoSeguranca { get; private set; }

        public PagamentoStatus Status { get; set; }

        public PedidoPagamento(Guid pedidoId, double valor, string numeroCartao, DateTime vencimentoCartao, int codigoSeguranca)
        {
            Id = Guid.NewGuid();
            PedidoId = pedidoId;
            Valor = valor;
            NumeroCartao = numeroCartao;
            VencimentoCartao = vencimentoCartao;
            CodigoSeguranca = codigoSeguranca;
            Status = PagamentoStatus.Aguardando;
        }

        public bool ValidarCartao()
        {
            if (string.IsNullOrWhiteSpace(NumeroCartao))
                return false;

            if (NumeroCartao.Length != 16)
                return false;

            var primeiroDiaMes = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            if (VencimentoCartao < primeiroDiaMes)
                return false;

            return true;
        }

        public void AtualizarStatus(PagamentoStatus status)
        {
            Status = status;
        }



    }
}
