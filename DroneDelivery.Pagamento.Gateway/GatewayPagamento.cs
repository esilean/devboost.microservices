using DroneDelivery.Pagamento.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace DroneDelivery.Pagamento.Gateway
{
    public class GatewayPagamento : IGatewayPagamento
    {
        private readonly ILogger<GatewayPagamento> _logger;
        public GatewayPagamento(ILogger<GatewayPagamento> logger)
        {
            _logger = logger;
        }

        public void EnviarParaPagamento()
        {
            _logger.LogInformation("Enviando para pagamento");
        }
    }
}
