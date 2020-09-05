using DroneDelivery.Domain.Core.Commands;
using DroneDelivery.Domain.Core.Domain;
using DroneDelivery.Domain.Core.Queries;
using System.Threading.Tasks;

namespace DroneDelivery.Domain.Core.Mediator
{
    public interface IMediatorHandler
    {
        Task<ResponseResult> RequestQuery<T>(T queryFilter) where T : QueryFilter;

        Task<ResponseResult> SendCommand<T>(T command) where T : Command;

    }
}
