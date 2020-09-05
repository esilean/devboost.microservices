using DroneDelivery.Domain.Core.Commands;
using DroneDelivery.Domain.Core.Domain;
using DroneDelivery.Domain.Core.Mediator;
using DroneDelivery.Domain.Core.Queries;
using MediatR;
using System.Threading.Tasks;

namespace DroneDelivery.Bus
{

    public class MediatorHandler : IMediatorHandler
    {

        private readonly IMediator _mediator;

        public MediatorHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public Task<ResponseResult> SendCommand<T>(T command) where T : Command
        {
            return _mediator.Send(command);
        }

        public Task<ResponseResult> RequestQuery<T>(T queryFilter) where T : QueryFilter
        {
            return _mediator.Send(queryFilter);
        }
    }
}
