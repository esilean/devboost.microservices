using DroneDelivery.Shared.Domain.Core.Events.Pedidos;
using DroneDelivery.Shared.Infra.Interfaces;
using DroneDelivery.Shared.Utility.Events;
using System.Net.Http;
using System.Threading.Tasks;

namespace DroneDelivery.Shared.Infra.Eventos
{
    public class PedidoPagamentoEvent : IPedidoPagamentoEvent
    {

        private readonly IHttpClientFactory _factory;

        public PedidoPagamentoEvent(IHttpClientFactory factory)
        {
            _factory = factory;
        }

        public async Task<bool> EnviarPedido(PedidoCriadoEvent @event)
        {

            var client = _factory.CreateClient(HttpClientName.PagamentoEndPoint);
            var response = await client.PostAsJsonAsync("/api/pedidos", @event);

            return response.EnsureSuccessStatusCode().IsSuccessStatusCode;
        }

    }
}
