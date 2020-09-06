using DroneDelivery.Pagamento.Application.Commands.Pagamentos;
using DroneDelivery.Pagamento.Data.Repositorios.Interfaces;
using DroneDelivery.Pagamento.Domain.Enums;
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

namespace DroneDelivery.Pagamento.Application.CommandHandlers.Pagamentos
{
    public class WebhookPagamentoHandler : ValidatorResponse, IRequestHandler<WebhookPagamentoCommand, ResponseResult>
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IEventBus _eventBus;

        public WebhookPagamentoHandler(IUnitOfWork unitOfWork, IEventBus eventBus)
        {
            _unitOfWork = unitOfWork;
            _eventBus = eventBus;
        }


        public async Task<ResponseResult> Handle(WebhookPagamentoCommand request, CancellationToken cancellationToken)
        {
            request.Validate();
            if (request.Notifications.Any())
            {
                _response.AddNotifications(request.Notifications);
                return _response;
            }

            var pedido = await _unitOfWork.Pedidos.ObterPorIdAsync(request.PedidoId);
            if (pedido == null)
            {
                _response.AddNotification(new Notification("pagamento", Erros.ErroPedido_NaoEncontrado));
                return _response;
            }

            if (pedido.Status != PedidoStatus.ProcessandoPagamento)
            {
                _response.AddNotification(new Notification("pagamento", Erros.ErroPedido_PedidoNaoEstaProcessandoPgto));
                return _response;
            }

            var ultimoPgtoAberto = pedido.Pagamentos.FirstOrDefault(x => x.DataConfirmacao == null);
            if (ultimoPgtoAberto == null)
            {
                _response.AddNotification(new Notification("pagamento", Erros.ErroPagamento_UltimoRegistroPgto));
                return _response;
            }

            if (request.Status == PagamentoStatus.Aprovado)
            {
                pedido.AtualizarStatus(PedidoStatus.Pago);
                ultimoPgtoAberto.AprovarPagamento();
            }
            else
            {
                pedido.AtualizarStatus(PedidoStatus.Cancelado);
                ultimoPgtoAberto.ReprovarPagamento();
            }

            //publica o evento para o bus
            await _eventBus.Publish(new PedidoStatusAtualizadoEvent(pedido.Id, pedido.Status));

            await _unitOfWork.SaveAsync();
            return _response;
        }
    }
}
