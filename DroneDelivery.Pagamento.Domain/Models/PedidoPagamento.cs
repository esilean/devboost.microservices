using DroneDelivery.Pagamento.Domain.Enums;
using System;

namespace DroneDelivery.Pagamento.Domain.Models
{
    public class PedidoPagamento
    {

        public Guid Id { get; private set; }

        public Pedido Pedido { get; private set; }

        public double ValorPago { get; private set; }

        public string NumeroCartao { get; private set; }

        public DateTime VencimentoCartao { get; private set; }

        public int CodigoSeguranca { get; private set; }

        public PagamentoStatus Status { get; private set; }


        protected PedidoPagamento() { }

        public PedidoPagamento(Pedido pedido, string numeroCartao, DateTime vencimentoCartao, int codigoSeguranca)
        {
            Id = Guid.NewGuid();
            Pedido = pedido;
            ValorPago = pedido.Valor;
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
