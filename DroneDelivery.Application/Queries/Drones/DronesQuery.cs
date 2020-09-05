using DroneDelivery.Domain.Core.Domain;
using DroneDelivery.Domain.Core.Queries;
using MediatR;

namespace DroneDelivery.Application.Queries.Drones
{
    public class DronesQuery : QueryFilter, IRequest<ResponseResult> { }
}
