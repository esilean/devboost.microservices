using System;
using System.Threading.Tasks;

namespace DroneDelivery.Pagamento.Data.Repositorios.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IPedidoRepository Pedidos { get; }

        Task SaveAsync();

    }
}
