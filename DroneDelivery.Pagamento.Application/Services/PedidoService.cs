using DroneDelivery.Pagamento.Application.Dtos;
using DroneDelivery.Pagamento.Application.Interfaces;
using DroneDelivery.Pagamento.Data.Repositorios.Interfaces;
using DroneDelivery.Pagamento.Domain.Models;
using System.Threading.Tasks;

namespace DroneDelivery.Pagamento.Application.Services
{
    public class PedidoService : IPedidoService
    {
        private readonly IUnitOfWork _unitOfWork;
        public PedidoService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task CriarPedido(CriarPedidoDto criarPedidoDto)
        {
            //validacoes do DTO


            var pedido = new Pedido(criarPedidoDto.Id, criarPedidoDto.Valor);


            await _unitOfWork.Pedidos.AdicionarAsync(pedido);
            await _unitOfWork.SaveAsync();

        }
    }
}
