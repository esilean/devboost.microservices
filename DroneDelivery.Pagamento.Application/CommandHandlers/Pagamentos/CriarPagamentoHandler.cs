using DroneDelivery.Pagamento.Application.Commands.Pagamentos;
using DroneDelivery.Pagamento.Application.Interfaces;
using DroneDelivery.Pagamento.Data.Repositorios.Interfaces;
using DroneDelivery.Pagamento.Domain.Models;
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
    public class CriarPagamentoHandler : ValidatorResponse, IRequestHandler<CriarPagamentoCommand, ResponseResult>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEventBus _eventBus;
        private readonly IGatewayPagamento _gatewayPagamento;

        public CriarPagamentoHandler(IUnitOfWork unitOfWork, IEventBus eventBus, IGatewayPagamento gatewayPagamento)
        {
            _unitOfWork = unitOfWork;
            _eventBus = eventBus;
            _gatewayPagamento = gatewayPagamento;
        }


        public async Task<ResponseResult> Handle(CriarPagamentoCommand request, CancellationToken cancellationToken)
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

            if (pedido.Status != PedidoStatus.AguardandoPagamento)
            {
                _response.AddNotification(new Notification("pagamento", Erros.ErroPedido_PedidoNaoEstaAguardandoPgto));
                return _response;
            }


            var pagamento = PedidoPagamento.Criar(
                pedido,
                request.NumeroCartao,
                request.VencimentoCartao,
                request.CodigoSeguranca);

            if (!pagamento.ValidarCartao())
            {
                _response.AddNotification(new Notification("pagamento", Erros.ErroPagamento_CartaoInvalido));
                return _response;
            }



            /******************/
            /******************/
            /******************/
            /* MOCK - Gateway de Pagamento*/
            var token = await _gatewayPagamento.EnviarParaPagamento(pedido);
            pagamento.AdicionarToken(token);

            /******************/
            /******************/
            /******************/

            pedido.AdicionarPagamento(pagamento);
            pedido.AtualizarStatus(PedidoStatus.ProcessandoPagamento);

            //publica o evento para o bus
            await _eventBus.Publish(new PedidoStatusAtualizadoEvent(pedido.Id, pedido.Status));


            await _unitOfWork.Pedidos.AdicionarPagamentoAsync(pagamento);
            await _unitOfWork.SaveAsync();

            return _response;
        }
    }
}
