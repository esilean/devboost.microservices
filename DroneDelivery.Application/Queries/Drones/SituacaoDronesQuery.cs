using DroneDelivery.Shared.Domain.Core.Domain;
using DroneDelivery.Shared.Domain.Core.Queries;
using MediatR;

namespace DroneDelivery.Application.Queries.Drones
{
    public class SituacaoDronesQuery : Query, IRequest<ResponseResult> { }
}
