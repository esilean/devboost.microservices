using DroneDelivery.Shared.Domain.Core.Events.Pedidos;
using DroneDelivery.Shared.Infra.Interfaces;
using DroneDelivery.Shared.Utility.Events;
using System.Net.Http;
using System.Threading.Tasks;

namespace DroneDelivery.Shared.Infra.HttpFactories
{
    public class PagamentoHttpFactory : IPagamentoHttpFactory
    {

        private readonly IHttpClientFactory _factory;

        public PagamentoHttpFactory(IHttpClientFactory factory)
        {
            _factory = factory;
        }

        public async Task<bool> EnviarPedidoParaPagamento(PedidoCriadoEvent @event)
        {

            var client = _factory.CreateClient(HttpClientName.PagamentoEndPoint);
            var response = await client.PostAsJsonAsync("/api/pedidos", @event);

            return response.EnsureSuccessStatusCode().IsSuccessStatusCode;
        }

    }
}
