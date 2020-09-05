using DroneDelivery.Pagamento.Domain.Models;
using System;
using System.Threading.Tasks;

namespace DroneDelivery.Pagamento.Data.Repositorios.Interfaces
{
    public interface IPedidoRepository
    {
        Task<Pedido> ObterPorIdAsync(Guid id);

        Task AdicionarAsync(Pedido pedido);

        Task AdicionarPagamentoAsync(PedidoPagamento pedidoPagamento);
    }
}
