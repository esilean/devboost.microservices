using DroneDelivery.Pagamento.Application.Interfaces;
using DroneDelivery.Pagamento.Domain.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace DroneDelivery.Pagamento.Gateway
{
    public class GatewayPagamento : IGatewayPagamento
    {

        private readonly HttpClient _httpClient;

        public GatewayPagamento()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new System.Uri("https://demo8366776.mockable.io/")
            };
        }

        public async Task<string> EnviarParaPagamento(Pedido pedido)
        {
            var response = await _httpClient.GetAsync("gateway-pgdrone");
            if (!response.IsSuccessStatusCode)
                return default;

            var result = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<TokenGateway>(result).Token;

        }
    }

    public class TokenGateway
    {
        public string Token { get; set; }
    }
}
