using DroneDelivery.Pagamento.Application.Dtos;
using DroneDelivery.Pagamento.Application.Interfaces;
using System.Net.Http;
using System.Threading.Tasks;

namespace DroneDelivery.Shared.Infra
{
    public class EnviarRespostaPagamento : IEnviarRespostaPagamento
    {

        private readonly IHttpClientFactory _factory;

        public EnviarRespostaPagamento(IHttpClientFactory factory)
        {
            _factory = factory;
        }

        public async Task<bool> EnviarConfirmacaoPagamento(CriarRepostaPagamentoDto criarRepostaPagamentoDto)
        {

            var client = _factory.CreateClient("pedidos");
            var response = await client.PostAsJsonAsync("/api/pedidos/atualizarstatus", criarRepostaPagamentoDto);

            return response.IsSuccessStatusCode;
        }

    }
}
