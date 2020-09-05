using DroneDelivery.Domain.Core.Domain;
using DroneDelivery.Domain.Core.Queries;
using MediatR;

namespace DroneDelivery.Application.Queries.Pedidos
{
    public class PedidosQuery : QueryFilter, IRequest<ResponseResult> { }
}
