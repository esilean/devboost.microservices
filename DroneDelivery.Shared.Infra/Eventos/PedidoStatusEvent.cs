using DroneDelivery.Shared.Domain.Core.Events.Pedidos;
using DroneDelivery.Shared.Infra.Interfaces;
using DroneDelivery.Shared.Utility.Events;
using System.Net.Http;
using System.Threading.Tasks;

namespace DroneDelivery.Shared.Infra.Eventos
{
    public class PedidoStatusEvent : IPedidoStatusEvent
    {
        private readonly IHttpClientFactory _factory;

        public PedidoStatusEvent(IHttpClientFactory factory)
        {
            _factory = factory;
        }

        public async Task<bool> AtualizarStatus(PedidoStatusAtualizadoEvent @event)
        {

            var client = _factory.CreateClient(HttpClientName.PedidoEndPoint);
            var response = await client.PostAsJsonAsync("/api/pedidos/atualizarstatus", @event);

            return response.EnsureSuccessStatusCode().IsSuccessStatusCode;
        }
    }
}
