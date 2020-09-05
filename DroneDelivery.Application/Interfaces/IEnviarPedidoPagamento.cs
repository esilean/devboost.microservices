using DroneDelivery.Application.Dtos.Pedido;
using System.Threading.Tasks;

namespace DroneDelivery.Application.Interfaces
{
    public interface IEnviarPedidoPagamento
    {
        Task<bool> ReceberPedidoPagamento(CriarPedidoDto criarPedidoDto);
    }
}
