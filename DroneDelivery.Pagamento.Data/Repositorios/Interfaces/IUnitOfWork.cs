using System;
using System.Threading.Tasks;

namespace DroneDelivery.Pagamento.Data.Repositorios.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IPedidoPagamentoRepository PedidoPagamentos { get; }

        Task SaveAsync();

    }
}
