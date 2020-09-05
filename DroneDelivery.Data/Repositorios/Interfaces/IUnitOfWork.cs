using System;
using System.Threading.Tasks;

namespace DroneDelivery.Data.Repositorios.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IPedidoRepository Pedidos { get; }
        IDroneRepository Drones { get; }
        IUsuarioRepository Usuarios { get; }
        Task SaveAsync();

    }
}
