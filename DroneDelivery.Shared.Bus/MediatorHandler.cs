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
        private readonly IPedidoPagamentoEvent _pedidoPagamentoEvent;
        private readonly IPedidoStatusEvent _pedidoStatusEvent;

        public MediatorHandler(
                                IMediator mediator,
                                IPedidoPagamentoEvent pedidoPagamentoEvent,
                                IPedidoStatusEvent pedidoStatusEvent)
        {
            _mediator = mediator;
            _pedidoPagamentoEvent = pedidoPagamentoEvent;
            _pedidoStatusEvent = pedidoStatusEvent;
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
                await _pedidoPagamentoEvent.EnviarPedido(@event as PedidoCriadoEvent);
            else if (eventType == typeof(PedidoStatusAtualizadoEvent))
                await _pedidoStatusEvent.AtualizarStatus(@event as PedidoStatusAtualizadoEvent);

        }
    }
}
