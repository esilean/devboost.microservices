using DroneDelivery.Shared.Domain.Core.Commands;
using DroneDelivery.Shared.Domain.Core.Domain;
using DroneDelivery.Shared.Domain.Core.Events;
using DroneDelivery.Shared.Domain.Core.Queries;
using System.Threading.Tasks;

namespace DroneDelivery.Shared.Domain.Core.Bus
{
    public interface IEventBus
    {
        Task<ResponseResult> RequestQuery<T>(T query) where T : Query;

        Task<ResponseResult> SendCommand<T>(T command) where T : Command;

        Task Publish<T>(T @event) where T : Event;

    }
}
