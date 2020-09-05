using DroneDelivery.Pagamento.Domain.Enums;
using System;
using System.Collections.Generic;

namespace DroneDelivery.Pagamento.Domain.Models
{
    public class Pedido
    {
        public Guid Id { get; private set; }

        public Guid PedidoId { get; private set; }

        public double Valor { get; set; }

        public PedidoStatus Status { get; private set; }

        public List<PedidoPagamento> Pagamentos { get; private set; }

        public Pedido(Guid pedidoId, double valor)
        {
            Id = Guid.NewGuid();
            PedidoId = pedidoId;
            Valor = valor;
            Status = PedidoStatus.AguardandoPagamento;
            Pagamentos = new List<PedidoPagamento>();
        }

        public void AdicionarPagamento(PedidoPagamento pedidoPagamento)
        {
            Pagamentos.Add(pedidoPagamento);
        }

        public void AtualizarStatus(PedidoStatus status)
        {
            Status = status;
        }
    }
}
