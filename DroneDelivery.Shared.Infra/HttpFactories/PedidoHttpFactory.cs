using DroneDelivery.Shared.Domain.Core.Events.Pedidos;
using DroneDelivery.Shared.Infra.Interfaces;
using DroneDelivery.Shared.Utility.Events;
using System.Net.Http;
using System.Threading.Tasks;

namespace DroneDelivery.Shared.Infra.HttpFactories
{
    public class PedidoHttpFactory : IPedidoHttpFactory
    {
        private readonly IHttpClientFactory _factory;

        public PedidoHttpFactory(IHttpClientFactory factory)
        {
            _factory = factory;
        }

        public async Task<bool> AtualizarPedidoStatus(PedidoStatusAtualizadoEvent @event)
        {

            var client = _factory.CreateClient(HttpClientName.PedidoEndPoint);
            var response = await client.PostAsJsonAsync("/api/pedidos/atualizarstatus", @event);

            return response.EnsureSuccessStatusCode().IsSuccessStatusCode;
        }
    }
}
