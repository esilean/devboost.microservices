using DroneDelivery.Pagamento.Application.Dtos;
using System.Threading.Tasks;

namespace DroneDelivery.Pagamento.Application.Interfaces
{
    public interface IEnviarRespostaPagamento
    {
        Task<bool> EnviarConfirmacaoPagamento(CriarRepostaPagamentoDto criarRepostaPagamentoDto);
    }
}
