using DroneDelivery.Pagamento.Domain.Models;
using System.Threading.Tasks;

namespace DroneDelivery.Pagamento.Application.Interfaces
{
    public interface IGatewayPagamento
    {
        Task<string> EnviarParaPagamento(Pedido pedido);
    }
}
