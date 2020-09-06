using DroneDelivery.Pagamento.Domain.Enums;
using System;

namespace DroneDelivery.Pagamento.Domain.Models
{
    public class PedidoPagamento
    {

        public Guid Id { get; private set; }

        public Pedido Pedido { get; private set; }

        public double ValorPago { get; private set; }

        public DateTime DataCriacao { get; private set; }

        public string NumeroCartao { get; private set; }

        public DateTime VencimentoCartao { get; private set; }

        public int CodigoSeguranca { get; private set; }

        public PagamentoStatus Status { get; private set; }

        public DateTime? DataConfirmacao { get; set; }

        public string Token { get; private set; }


        protected PedidoPagamento() { }

        private PedidoPagamento(Pedido pedido, string numeroCartao, DateTime vencimentoCartao, int codigoSeguranca)
        {
            Id = Guid.NewGuid();
            Pedido = pedido;
            ValorPago = pedido.Valor;
            DataCriacao = DateTime.Now;
            NumeroCartao = numeroCartao;
            VencimentoCartao = vencimentoCartao;
            CodigoSeguranca = codigoSeguranca;
            Status = PagamentoStatus.Aguardando;
        }

        public static PedidoPagamento Criar(Pedido pedido, string numeroCartao, DateTime vencimentoCartao, int codigoSeguranca)
        {
            return new PedidoPagamento(pedido, numeroCartao, vencimentoCartao, codigoSeguranca);
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

        public void AprovarPagamento()
        {
            DataConfirmacao = DateTime.Now;
            Status = PagamentoStatus.Aprovado;
        }

        public void ReprovarPagamento()
        {
            DataConfirmacao = DateTime.Now;
            Status = PagamentoStatus.Reprovado;
        }

        public void AdicionarToken(string token)
        {
            Token = token;
        }

    }
}
