using DroneDelivery.Pagamento.Application.Dtos;
using System.Threading.Tasks;

namespace DroneDelivery.Pagamento.Application.Interfaces
{
    public interface IPedidoService
    {
        Task CriarPedido(CriarPedidoDto criarPedidoDto);
    }
}
