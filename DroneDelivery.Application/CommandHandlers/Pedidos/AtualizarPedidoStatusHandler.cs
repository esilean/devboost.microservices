using DroneDelivery.Application.Commands.Pedidos;
using DroneDelivery.Application.Services.Interfaces;
using DroneDelivery.Data.Repositorios.Interfaces;
using DroneDelivery.Shared.Domain.Core.Domain;
using DroneDelivery.Shared.Domain.Core.Enums;
using DroneDelivery.Shared.Domain.Core.Validator;
using DroneDelivery.Shared.Utility.Messages;
using Flunt.Notifications;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace DroneDelivery.Application.CommandHandlers.Pedidos
{
    public class AtualizarPedidoStatusHandler : ValidatorResponse, IRequestHandler<AtualizarPedidoStatusCommand, ResponseResult>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAssociarPedidoDroneService _associarPedidoDroneService;

        public AtualizarPedidoStatusHandler(IUnitOfWork unitOfWork, IAssociarPedidoDroneService associarPedidoDroneService)
        {
            _unitOfWork = unitOfWork;
            _associarPedidoDroneService = associarPedidoDroneService;
        }

        public async Task<ResponseResult> Handle(AtualizarPedidoStatusCommand request, CancellationToken cancellationToken)
        {
            request.Validate();
            if (request.Invalid)
            {
                _response.AddNotifications(request.Notifications);
                return _response;
            }

            var pedido = await _unitOfWork.Pedidos.ObterPorIdAsync(request.Id);
            if (pedido == null)
            {
                _response.AddNotification(new Notification("pedido", Erros.ErroPedido_NaoEncontrado));
                return _response;
            }

            if (pedido.Status == PedidoStatus.Entregue)
            {
                _response.AddNotification(new Notification("pedido", Erros.ErroPedido_PedidoJaFoiEntregue));
                return _response;
            }

            //se o status enviado para atualizar o pedido for pago, devemos procurar um drone para ele ou deixa-lo em espera
            if (request.Status == PedidoStatus.Pago)
            {
                var drone = await _associarPedidoDroneService.AssociarPedidoAoDrone(pedido);
                if (drone == null)
                {
                    pedido.AtualizarStatusPedido(PedidoStatus.AguardandoDrone);
                }
                else
                {
                    pedido.AtualizarStatusPedido(PedidoStatus.EmEntrega);
                    pedido.AssociarDrone(drone);
                }
            }
            else
            {
                pedido.AtualizarStatusPedido(request.Status);
            }


            await _unitOfWork.SaveAsync();

            return _response;
        }


    }
}
