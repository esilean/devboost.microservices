using DroneDelivery.Shared.Domain.Core.Domain;
using DroneDelivery.Shared.Domain.Core.Enums;
using System;
using System.Collections.Generic;

namespace DroneDelivery.Pagamento.Domain.Models
{
    public class Pedido : Entity, IAggregateRoot
    {

        public double Valor { get; private set; }

        public PedidoStatus Status { get; private set; }

        private readonly List<PedidoPagamento> _pagamentos;

        public ICollection<PedidoPagamento> Pagamentos => _pagamentos;

        private Pedido(Guid id, double valor)
        {
            Id = id;
            Valor = valor;
            Status = PedidoStatus.AguardandoPagamento;
            _pagamentos = new List<PedidoPagamento>();
        }

        public static Pedido Criar(Guid id, double valor)
        {
            return new Pedido(id, valor);
        }

        public void AdicionarPagamento(PedidoPagamento pedidoPagamento)
        {
            _pagamentos.Add(pedidoPagamento);
        }

        public void AtualizarStatus(PedidoStatus status)
        {
            Status = status;
        }
    }
}
