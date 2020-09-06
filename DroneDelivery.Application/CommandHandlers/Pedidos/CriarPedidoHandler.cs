using DroneDelivery.Application.Commands.Pedidos;
using DroneDelivery.Application.Interfaces;
using DroneDelivery.Data.Repositorios.Interfaces;
using DroneDelivery.Domain.Models;
using DroneDelivery.Shared.Domain.Core.Bus;
using DroneDelivery.Shared.Domain.Core.Domain;
using DroneDelivery.Shared.Domain.Core.Enums;
using DroneDelivery.Shared.Domain.Core.Events.Pedidos;
using DroneDelivery.Shared.Domain.Core.Validator;
using DroneDelivery.Shared.Utility.Messages;
using Flunt.Notifications;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DroneDelivery.Application.CommandHandlers.Pedidos
{
    public class CriarPedidoHandler : ValidatorResponse, IRequestHandler<CriarPedidoCommand, ResponseResult>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUsuarioAutenticado _usuarioAutenticado;
        private readonly IEventBus _eventBus;

        public CriarPedidoHandler(IUnitOfWork unitOfWork, IUsuarioAutenticado usuarioAutenticado, IEventBus eventBus)
        {
            _unitOfWork = unitOfWork;
            _usuarioAutenticado = usuarioAutenticado;
            _eventBus = eventBus;
        }


        public async Task<ResponseResult> Handle(CriarPedidoCommand request, CancellationToken cancellationToken)
        {
            request.Validate();
            if (request.Notifications.Any())
            {
                _response.AddNotifications(request.Notifications);
                return _response;
            }

            var clienteId = _usuarioAutenticado.GetCurrentId();
            var cliente = await _unitOfWork.Usuarios.ObterPorIdAsync(clienteId);
            if (cliente == null)
            {
                _response.AddNotification(new Notification("pedido", Erros.ErroCliente_NaoEncontrado));
                return _response;
            }

            var pedido = Pedido.Criar(request.Peso, request.Valor, cliente);
            pedido.AtualizarStatusPedido(PedidoStatus.AguardandoPagamento);

            //publica o evento para o bus
            await _eventBus.Publish(new PedidoCriadoEvent(pedido.Id, pedido.Valor));

            await _unitOfWork.Pedidos.AdicionarAsync(pedido);
            await _unitOfWork.SaveAsync();
            return _response;
        }
    }
}
