using DroneDelivery.Shared.Domain.Core.Events.Pedidos;
using System.Threading.Tasks;

namespace DroneDelivery.Shared.Infra.Interfaces
{
    public interface IPagamentoHttpFactory
    {
        Task<bool> EnviarPedidoParaPagamento(PedidoCriadoEvent @event);
    }
}
