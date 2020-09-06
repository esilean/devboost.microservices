using DroneDelivery.Shared.Domain.Core.Bus;
using DroneDelivery.Shared.Domain.Core.Commands;
using DroneDelivery.Shared.Domain.Core.Domain;
using DroneDelivery.Shared.Domain.Core.Events;
using DroneDelivery.Shared.Domain.Core.Events.Pedidos;
using DroneDelivery.Shared.Domain.Core.Queries;
using DroneDelivery.Shared.Infra.Interfaces;
using MediatR;
using System.Threading.Tasks;

namespace DroneDelivery.Shared.Bus
{
    public class MediatorHandler : IEventBus
    {

        private readonly IMediator _mediator;
        private readonly IPagamentoHttpFactory _pagamentoHttpFactory;
        private readonly IPedidoHttpFactory _pedidoHttpFactory;

        public MediatorHandler(
                                IMediator mediator,
                                IPagamentoHttpFactory pagamentoHttpFactory,
                                IPedidoHttpFactory pedidoHttpFactory)
        {
            _mediator = mediator;
            _pagamentoHttpFactory = pagamentoHttpFactory;
            _pedidoHttpFactory = pedidoHttpFactory;
        }

        public Task<ResponseResult> SendCommand<T>(T command) where T : Command
        {
            return _mediator.Send(command);
        }

        public Task<ResponseResult> RequestQuery<T>(T query) where T : Query
        {
            return _mediator.Send(query);
        }

        public async Task Publish<T>(T @event) where T : Event
        {
            var eventType = @event.GetType();

            if (eventType == typeof(PedidoCriadoEvent))
                await _pagamentoHttpFactory.EnviarPedidoParaPagamento(@event as PedidoCriadoEvent);
            else if (eventType == typeof(PedidoStatusAtualizadoEvent))
                await _pedidoHttpFactory.AtualizarPedidoStatus(@event as PedidoStatusAtualizadoEvent);

        }
    }
}
