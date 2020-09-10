using DroneDelivery.Shared.Domain.Core.Events.Pedidos;
using DroneDelivery.Shared.Infra.Interfaces;
using System.Threading.Tasks;

namespace DroneDelivery.Pagamento.Api.Tests.MockHttpFacotry
{
    public class MockPedidoHttpFactory : IPedidoHttpFactory
    {
        public Task<bool> AtualizarPedidoStatus(PedidoStatusAtualizadoEvent @event)
        {
            return Task.FromResult(true);
        }
    }
}
