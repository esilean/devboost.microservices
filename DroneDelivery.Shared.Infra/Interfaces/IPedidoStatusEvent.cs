using DroneDelivery.Shared.Domain.Core.Events.Pedidos;
using System.Threading.Tasks;

namespace DroneDelivery.Shared.Infra.Interfaces
{
    public interface IPedidoStatusEvent
    {
        Task<bool> AtualizarStatus(PedidoStatusAtualizadoEvent @event);
    }
}
