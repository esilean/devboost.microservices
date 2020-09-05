using DroneDelivery.Pagamento.Domain.Models;
using System.Threading.Tasks;

namespace DroneDelivery.Pagamento.Data.Repositorios.Interfaces
{
    public interface IPedidoPagamentoRepository
    {
        Task AdicionarAsync(PedidoPagamento pedidoPagamento);
    }
}
