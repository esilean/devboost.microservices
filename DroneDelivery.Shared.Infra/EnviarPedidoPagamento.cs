using DroneDelivery.Application.Dtos.Pedido;
using DroneDelivery.Application.Interfaces;
using System.Net.Http;
using System.Threading.Tasks;

namespace DroneDelivery.Shared.Infra
{
    public class EnviarPedidoPagamento : IEnviarPedidoPagamento
    {
        private readonly IHttpClientFactory _factory;

        public EnviarPedidoPagamento(IHttpClientFactory factory)
        {
            _factory = factory;
        }

        public async Task<bool> ReceberPedidoPagamento(CriarPedidoDto criarPedidoDto)
        {

            var client = _factory.CreateClient("pagamentos");
            var response = await client.PostAsJsonAsync("/api/pedidos", criarPedidoDto);

            return response.IsSuccessStatusCode;
        }
    }
}
