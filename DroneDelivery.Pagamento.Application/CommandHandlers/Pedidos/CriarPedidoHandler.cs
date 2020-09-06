using DroneDelivery.Pagamento.Application.Commands.Pedidos;
using DroneDelivery.Pagamento.Data.Repositorios.Interfaces;
using DroneDelivery.Pagamento.Domain.Models;
using DroneDelivery.Shared.Domain.Core.Domain;
using DroneDelivery.Shared.Domain.Core.Validator;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DroneDelivery.Pagamento.Application.CommandHandlers.Pedidos
{
    public class CriarPedidoHandler : ValidatorResponse, IRequestHandler<CriarPedidoCommand, ResponseResult>
    {

        private readonly IUnitOfWork _unitOfWork;
        public CriarPedidoHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseResult> Handle(CriarPedidoCommand request, CancellationToken cancellationToken)
        {
            request.Validate();
            if (request.Notifications.Any())
            {
                _response.AddNotifications(request.Notifications);
                return _response;
            }

            var pedido = Pedido.Criar(request.Id, request.Valor);

            await _unitOfWork.Pedidos.AdicionarAsync(pedido);
            await _unitOfWork.SaveAsync();

            return _response;
        }
    }
}
