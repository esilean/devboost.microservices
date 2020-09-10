using DroneDelivery.Shared.Domain.Core.Events.Pedidos;
using DroneDelivery.Shared.Infra.Interfaces;
using System.Threading.Tasks;

namespace DroneDelivery.Api.Tests.MockHttpFactory
{
    public class MockPagamentoHttpFactory : IPagamentoHttpFactory
    {
        public Task<bool> EnviarPedidoParaPagamento(PedidoCriadoEvent @event)
        {
            return Task.FromResult(true);
        }
    }
}
