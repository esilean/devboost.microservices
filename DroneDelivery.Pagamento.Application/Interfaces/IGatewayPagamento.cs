using System.Threading.Tasks;

namespace DroneDelivery.Pagamento.Application.Interfaces
{
    public interface IGatewayPagamento
    {
        void EnviarParaPagamento();
    }
}
