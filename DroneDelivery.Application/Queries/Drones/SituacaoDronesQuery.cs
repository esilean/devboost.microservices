using DroneDelivery.Domain.Core.Domain;
using DroneDelivery.Domain.Core.Queries;
using MediatR;

namespace DroneDelivery.Application.Queries.Drones
{
    public class SituacaoDronesQuery : QueryFilter, IRequest<ResponseResult> { }
}
